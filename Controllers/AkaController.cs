using Microsoft.AspNetCore.Mvc;

namespace aka.Controllers;

[ApiController]
[Route("/")]
public class AkaController : ControllerBase
{
    private readonly ILogger<AkaController> _logger;
    private readonly DataController _dataController;

    public AkaController(ILogger<AkaController> logger, DataController dataController)
    {
        _logger = logger;
        _dataController = dataController;
    }

    [HttpGet("all")]
    public IActionResult GetAllEntries()
    {
        return Ok(_dataController.data);
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        _logger.LogInformation("Get({id})", id);
        var ret = _dataController.GetKeyValue(id);
        return ret == null ? NotFound() : Redirect(ret);
    }

    [HttpPost]
    public IActionResult Post(string id, string value)
    {
        try
        {
            _dataController.SetKeyValue(id, value);
            _logger.LogInformation("Post({id}, {value})", id, value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Post({id}, {value})", id, value);
            return BadRequest();
        }
        return Ok(new Dictionary<string, string>{
            { "id", id },
            { "value", value }
        });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        try
        {
            _dataController.DeleteKeyValue(id);
            _logger.LogInformation("Delete({id})", id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Delete({id})", id);
            return BadRequest();
        }
        return Ok();
    }

}
