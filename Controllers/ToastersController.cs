using ToastApiReact.Models;
using ToastApiReact.Services;
using ToastApiReact.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;

namespace ToastApiReact.Controllers
{
    [Route("api/[controller]")]
    // I'm using attribute based routing here. The URL pattern is defined here
    // rather than in separate template
    // Route Attribute prefix here that defines base url for all action methods
    // within controller
    
    [ApiController]
    // Attribute - specifies that all derived types are serving api methods
    // including: attribute routing requirement, auto 400 responses, binding
    // source parameter inference, problem details for error status codes
    public class ToastersController : ControllerBase
    // deriving from Controller base instead of Controller, don't need
    // views support here
    {
        private readonly ToasterService _toasterService;

        public ToastersController(ToasterService toasterService)
        {
            _toasterService = toasterService;
        }

        // Get All Route, endpoint is api/toasters
        [HttpGet]
        // Attribute specifying to routing layer that GET is intended (http verb)
        public ActionResult<List<Toaster>> Get()
        // Action Result is a return type for controller actions
        // here wraps either the list of toaster or the ActionResult
        {

            _toasterService.Get();
            var toaster = _toasterService.Get();

            EventHandler.HandleFrontEndRequest("someString");
            // logging the request from the client to the log. Should probably
            // pass in toaster instead of "kazoo" but it isn't getting logged
            // anyway. 

            return toaster;
        }

        // Get One Route, endpoint is api/toasters/{id}

        [HttpGet("{id:length(24)}", Name = "GetToaster")]
        // Attribute specifying to routing layer that GET/{id} is intended (http verb)
        // Route Template attribute is here passed to http verb as a parameter
        //  :length(24) is a constraint to validate input. Can have multiple
        // constraints here for security (good idea).
        // Name parameter is for the route name. It's only used for generating
        // URLS. Useful for self-referencing URLs.

        public ActionResult<Toaster> Get(string id)
        // ActionResult here wraps either type (Toaster) or an ActionResult
        {
            var toaster = _toasterService.Get(id);
            // initiates variable equal to get method from toasterService

            if (toaster == null)
            {
                return NotFound();
                // sends a 404 if no toaster found
            }

            return toaster;
        }

        // Post Route, endpoint is api/toasters/new

        [HttpPost("new")]
        // Attribute specifying to routing layer that POST is intended (http verb)
        


        public ActionResult<Toaster> Create(Toaster toaster)
        {
            _toasterService.Create(toaster);
            // calls toasterService.Create method

            return CreatedAtRoute("GetToaster", new { id = toaster.Id.ToString() }, toaster);
            // sends 201 with info on new toaster in header
        }

        // Update Route, endpoint is api/toasters/{id}
        [HttpPut("{id:length(24)}")]
        // Attribute specifying to routing layer that PUT is intended (http verb
        public IActionResult Update(string id, Toaster toasterIn)
        // IAction Result is preferable here to ActionResult since the update
        // could result in multiple ActionResult return types. 
        // the Put action does the responding, the actionResult method chooses
        // what kind of response
        {
            // model binding is going on here. retrieving/providing/converting/updating
            // data is automated for the action

            var toaster = _toasterService.Get(id);

            if (toaster == null)
            {
                return NotFound();
            }

            var oldToaster = _toasterService.Get(id);

            _toasterService.Update(id, toasterIn);


            // running toaster lookup and update with most recent info for log

            // NOTE: it is logically possible for the toaster state to be updated in the DB
            // and still have the logging fail. This is something we can handle in the next iteration.
            EventHandler.HandleToasterStateChange(oldToaster, toasterIn);
            // call logging HandleToaster... function from EventHandler to log
            // the event
            return NoContent();
        }

        // Delete Route, endpoint is api/toasters/{id}

        [HttpDelete("{id:length(24)}")]
        // Attribute specifying to routing layer that DELETE is intended (http verb)
        // {id:------} is a route template passed to the attribute as a parameter
        public IActionResult Delete(string id)
        // IAction Result is preferable here to ActionResult since the update
        // could result in multiple ActionResult return types. "ActionResult
        // types represent various HTTP status codes".
        {
            var toaster = _toasterService.Get(id);
            // initiates variable equal to get method from toasterService

            if (toaster == null)
            {
                return NotFound();
                // sends 404 if no toaster found
            }

            _toasterService.Remove(toaster.Id);
            // if toaster is found, calls Remove method from service

            return NoContent();
            // sends an empty 204 response
        }
    }
}