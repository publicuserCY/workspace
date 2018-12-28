using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace Demo4OAuth.ResourceServer.Model
{
    public class PaginatedRequestModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            try
            {
                var requestModel = (PaginatedRequestModel)bindingContext.Model ?? new PaginatedRequestModel();
                if (!string.IsNullOrEmpty(bindingContext.ValueProvider.GetValue("Criteria").AttemptedValue))
                {
                    requestModel.Criteria = bindingContext.ValueProvider.GetValue("Criteria").AttemptedValue;
                }
                int pageIndex;
                if (int.TryParse(bindingContext.ValueProvider.GetValue("PageIndex").AttemptedValue, out pageIndex))
                {
                    requestModel.PageIndex = pageIndex;
                }
                int pageSize;
                if (int.TryParse(bindingContext.ValueProvider.GetValue("PageSize").AttemptedValue, out pageSize))
                {
                    requestModel.PageSize = pageSize;
                }
                if (!string.IsNullOrEmpty(bindingContext.ValueProvider.GetValue("OrderBy").AttemptedValue))
                {
                    requestModel.OrderBy = bindingContext.ValueProvider.GetValue("OrderBy").AttemptedValue;
                }
                if (!string.IsNullOrEmpty(bindingContext.ValueProvider.GetValue("Direction").AttemptedValue))
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
