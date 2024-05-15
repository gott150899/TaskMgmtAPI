using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections;
using Presentation.Model;

namespace Presentation.WebAPI.Middlewares
{
    public static class MiddlewareContext
    {
        public static IActionResult ModifyContentResult(int statusCode, object content)
        {
            string resultData = ModifyResult(statusCode, content);
            ContentResult response = new ContentResult
            {
                StatusCode = statusCode,
                Content = resultData
            };

            return response;
        }

        private static string ModifyResult(int statusCode, object body)
        {
            ResultModel result = new ResultModel();
            result.Success = IsSuccess(statusCode);
            result.Data = body;
            if (result.Success == false)
            {
                result.Data = null;


                if (body is string message)
                {
                    result.Error = message;
                }
                else if (body is ProblemDetails problemDetails)
                {
                    result.Error = problemDetails?.Detail;
                    if (string.IsNullOrEmpty(result.Error))
                    {
                        ICollection<string[]> problemDetailErrorValues = ((ValidationProblemDetails)problemDetails).Errors.Values;
                        foreach (string[] error in problemDetailErrorValues)
                        {
                            string?[] problemDetailErrorMessage = error.Cast<object>()
                                .Select(x => x?.ToString())
                                .ToArray();
                            result.Error += string.Join(", ", problemDetailErrorMessage);
                        }
                    }
                }
                // else if (body is ValidationProblemDetails validationProblemDetails)
                // {
                //     ICollection<string[]> problemDetailErrorValues = validationProblemDetails.Errors.Values;
                //     foreach (string[] error in problemDetailErrorValues)
                //     {
                //         string?[] problemDetailErrorMessage = error.Cast<object>()
                //             .Select(x => x?.ToString())
                //             .ToArray();
                //         result.Error += string.Join(", ", problemDetailErrorMessage);
                //     }
                // }
                else if (body is SerializableError serializableError)
                {
                    object serializableErrorValue = serializableError.Values.FirstOrDefault();
                    string?[] serializableErrorMessage = (serializableErrorValue as IEnumerable).Cast<object>()
                        .Select(x => x?.ToString())
                        .ToArray();
                    result.Error = string.Join(", ", serializableErrorMessage);
                }
            }

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(result, serializerSettings);
        }

        private static bool IsSuccess(int status)
        {
            return status == StatusCodes.Status200OK;
        }
    }
}
