using Goal.Infra.Crosscutting.Collections;
using Goal.Infra.Http.Controllers.Results;
using Microsoft.AspNetCore.Mvc;

namespace Goal.Infra.Http.Controllers;

public class ApiController : ControllerBase
{
    protected virtual IActionResult OkOrNotFound(object? value, string message)
    {
        return value is null
            ? NotFound(message)
            : Ok(value);
    }

    protected virtual IActionResult OkOrNotFound(object? value)
    {
        return value is null
            ? NotFound()
            : Ok(value);
    }

    protected virtual ActionResult<T> OkOrNotFound<T>(T? value, string message)
    {
        return value is null
            ? NotFound(message)
            : Ok(value);
    }

    protected virtual ActionResult<T> OkOrNotFound<T>(T? value)
    {
        return value is null
            ? NotFound()
            : Ok(value);
    }

    protected virtual ActionResult InternalServerError(object result)
        => new InternalServerErrorObjectResult(result);

    protected virtual ActionResult InternalServerError()
        => new InternalServerErrorResult();

    protected virtual ActionResult ServiceUnavailable(object result)
        => new ServiceUnavailableObjectResult(result);

    protected virtual ActionResult ServiceUnavailable()
        => new ServiceUnavailableResult();

    protected virtual OkPagedCollectionResult Paged(IPagedList collection)
        => new(collection);

    protected virtual OkPagedCollectionResult<T> Paged<T>(IPagedList<T> collection)
        => new(collection);
}
