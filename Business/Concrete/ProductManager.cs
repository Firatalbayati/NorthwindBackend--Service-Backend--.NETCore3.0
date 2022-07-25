using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using FluentValidation;

namespace Business.Concrete
{

    class ProductManager : IProductService
    {
        private IProductDAL _productDal;

        public ProductManager(IProductDAL productDal)
        {
            _productDal = productDal;
        }

        //[SecuredOperation("Admin, Product.List")]
        [CacheAspect(duration: 60)]
        [LogAspect(typeof(FileLogger))]
        [LogAspect(typeof(DatabaseLogger))]
        [PerformanceAspect(interval: 60)]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(filter: p => p.ProductId == productId));
        }

        //[SecuredOperation("Admin, Product.List")]
        [CacheAspect(duration: 60)]
        [LogAspect(typeof(FileLogger))]
        [LogAspect(typeof(DatabaseLogger))]
        [PerformanceAspect(interval:60)]
        public IDataResult<List<Product>> GetList()
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList().ToList());
        }

        //[SecuredOperation("Admin, Product.List")]
        [CacheAspect(duration: 60)]
        [LogAspect(typeof(FileLogger))] 
        [LogAspect(typeof(DatabaseLogger))]
        [PerformanceAspect(interval: 60)]
        public IDataResult<List<Product>> GetListByCategory(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList(filter: p => p.CategoryId == categoryId).ToList());
        }

        //[SecuredOperation("Admin, Product.List")]
        [ValidationAspect(typeof(ProductValidator), Priority = 1)]
        [PerformanceAspect(interval: 60)]
        [CacheRemoveAspect(pattern:"IProductService.Get")]
        [CacheRemoveAspect(pattern:"ICategoryService.Get")]
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName));
            if(result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        //[SecuredOperation("Admin, Product.List")]
        [PerformanceAspect(interval: 60)]
        [CacheRemoveAspect(pattern: "IProductService.Get")]
        [CacheRemoveAspect(pattern: "ICategoryService.Get")]
        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }

        //[SecuredOperation("Admin, Product.List")]
        [PerformanceAspect(interval: 60)]
        [CacheRemoveAspect(pattern: "IProductService.Get")]
        [CacheRemoveAspect(pattern: "ICategoryService.Get")]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }


        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetList(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }

            return new SuccessResult();
        }
    }
}
