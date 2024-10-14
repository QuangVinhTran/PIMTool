Put your controller at there
Example:

```
    [ApiController]
    [Route("projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService,
            IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> Get(int id)
        {
            var entity = await _projectService.GetAsync(id);
            return Ok(_mapper.Map<ProjectDto>(entity));
        }
    }
```
