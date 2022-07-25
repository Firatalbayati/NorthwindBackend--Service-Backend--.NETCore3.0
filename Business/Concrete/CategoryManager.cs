using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        //[SecuredOperation("Admin, Product.List")]
        [CacheAspect(duration: 60)]
        [LogAspect(typeof(FileLogger))]
        [LogAspect(typeof(DatabaseLogger))]
        [PerformanceAspect(interval: 60)]
        public IDataResult<Category> GetById(int categoryId)
        {
            return new SuccessDataResult<Category>(_categoryDal.Get(filter: c => c.CategoryId == categoryId));
        }

        //[SecuredOperation("Admin, Product.List")]
        [CacheAspect(duration: 1)]
        [LogAspect(typeof(FileLogger))]
        [LogAspect(typeof(DatabaseLogger))]
        [PerformanceAspect(interval: 60)]
        public IDataResult<List<Category>> GetList()
        {
            return new SuccessDataResult<List<Category>>(_categoryDal.GetList().ToList());
        }

        //[SecuredOperation("Admin, Product.List")]
        //[ValidationAspect(typeof(ProductValidator), Priority = 1)]
        [PerformanceAspect(interval: 60)]
        [CacheRemoveAspect(pattern: "IProductService.Get")]
        [CacheRemoveAspect(pattern: "ICategoryService.Get")]
        public IResult Add(Category category)
        {
            IResult result = BusinessRules.Run(CheckIfProductNameExists(category.CategoryName));
            if (result != null)
            {
                return result;
            }

            _categoryDal.Add(category);
            return new SuccessResult(Messages.CategoryAdded);
        }

        //[SecuredOperation("Admin, Product.List")]
        [PerformanceAspect(interval: 60)]
        [CacheRemoveAspect(pattern: "IProductService.Get")]
        [CacheRemoveAspect(pattern: "ICategoryService.Get")]
        public IResult Delete(Category category)
        {
            _categoryDal.Delete(category);
            return new SuccessResult(Messages.CategoryDeleted);
        }

        //[SecuredOperation("Admin, Product.List")]
        [PerformanceAspect(interval: 60)]
        [CacheRemoveAspect(pattern: "IProductService.Get")]
        [CacheRemoveAspect(pattern: "ICategoryService.Get")]
        public IResult Update(Category category)
        {
            _categoryDal.Update(category);
            return new SuccessResult(Messages.CategoryUpdated);
        }


        private IResult CheckIfProductNameExists(string categoryName)
        {
            var result = _categoryDal.GetList(c => c.CategoryName == categoryName).Any();
            if (result)
            {
                return new ErrorResult(Messages.CategoryNameAlreadyExists);
            }

            return new SuccessResult();
        }
    }
}
