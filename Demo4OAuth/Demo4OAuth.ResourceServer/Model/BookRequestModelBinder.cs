using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace Demo4OAuth.ResourceServer.Model
{
    public class BookRequestModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            try
            {
                if (bindingContext.ModelType != typeof(BookRequestModel))
                {
                    return false;
                }
                var requestModel = (BookRequestModel)bindingContext.Model ?? new BookRequestModel();

                if (bindingContext.ValueProvider.GetValue("Id") != null
                     && !string.IsNullOrEmpty(bindingContext.ValueProvider.GetValue("Id").AttemptedValue))
                {
                    requestModel.Id = bindingContext.ValueProvider.GetValue("Id").AttemptedValue;
                }
                if (bindingContext.ValueProvider.GetValue("Name") != null
                     && !string.IsNullOrEmpty(bindingContext.ValueProvider.GetValue("Name").AttemptedValue))
                {
                    requestModel.Name = bindingContext.ValueProvider.GetValue("Name").AttemptedValue;
                }
                if (bindingContext.ValueProvider.GetValue("Criteria") != null
                     && !string.IsNullOrEmpty(bindingContext.ValueProvider.GetValue("Criteria").AttemptedValue))
                {
                    requestModel.Criteria = bindingContext.ValueProvider.GetValue("Criteria").AttemptedValue;
                }
                int pageIndex;
                if (bindingContext.ValueProvider.GetValue("PageIndex") != null
                     && int.TryParse(bindingContext.ValueProvider.GetValue("PageIndex").AttemptedValue, out pageIndex))
                {
                    requestModel.PageIndex = pageIndex;
                }
                int pageSize;
                if (bindingContext.ValueProvider.GetValue("PageSize") != null
                     && int.TryParse(bindingContext.ValueProvider.GetValue("PageSize").AttemptedValue, out pageSize))
                {
                    requestModel.PageSize = pageSize;
                }
                if (bindingContext.ValueProvider.GetValue("OrderBy") != null
                     && !string.IsNullOrEmpty(bindingContext.ValueProvider.GetValue("OrderBy").AttemptedValue))
                {
                    requestModel.OrderBy = bindingContext.ValueProvider.GetValue("OrderBy").AttemptedValue;
                }
                if (bindingContext.ValueProvider.GetValue("Direction") != null
                    && !string.IsNullOrEmpty(bindingContext.ValueProvider.GetValue("Direction").AttemptedValue))
                {
                    requestModel.Direction = bindingContext.ValueProvider.GetValue("Direction").AttemptedValue;
                }
                bindingContext.Model = requestModel;
                return true;
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex.Message);
                return false;
            }
        }
    }
}
