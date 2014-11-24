using System.Web.Mvc;
using MediaDataApplication.AspNetMvcClient.Mappers;
using Ninject;

namespace MediaDataApplication.AspNetMvcClient.Controllers {

    public class BaseController : Controller {
        [Inject]
        public IMapper ModelMapper { get; set; }
    }

}