using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace CosmeticsStore.API.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)] // Hide from Swagger
public class MetadataController : ODataController
{
    [HttpGet("/odata/$metadata")]
    public IActionResult GetMetadata()
    {
        return Ok("OData Metadata");
    }
}
