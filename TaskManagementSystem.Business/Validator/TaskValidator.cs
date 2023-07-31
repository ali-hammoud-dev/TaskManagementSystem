using System.Net;
using Microsoft.IdentityModel.Tokens;
using TaskManagementSystem.Business.DTOs;
using TaskManagementSystem.Business.Validator.Interfaces;
using TaskManagementSystem.Common.Exceptions;

namespace TaskManagementSystem.Business.Validator;

public class TaskValidator : ITaskValidator
{
    public TaskValidator() { }
    public void ValidateUpdate(TaskDto task)
    {
        if (task.Id == 0)
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.BadRequest)
                .ErrorMessage("Bad Request (Id = 0)").Build();

        if(task.Title.IsNullOrEmpty())
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.BadRequest)
                .ErrorMessage("Title is Required").Build();

        if(string.IsNullOrEmpty(task.Description))
            throw new PlatformExceptionBuilder().StatusCode(HttpStatusCode.BadRequest)
                .ErrorMessage("Description is Required").Build();
    }
}

