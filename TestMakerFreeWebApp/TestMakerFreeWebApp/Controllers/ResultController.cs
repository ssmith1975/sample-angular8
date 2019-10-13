using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestMakerFreeWebApp.ViewModels;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : Controller
    {


        #region RESTful conventions method
        /// <summary>
        /// GET: api/result/{id}
        /// Retrieves the Result with the given {id}
        /// </summary>
        /// <param name="id">The ID of an existing Result</param>
        /// <returns>the Result with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Content("Not implemented (yet)!");

        } // end Get

        /// <summary>
        /// Adds a new Result to the Database
        /// </summary>
        /// <param name="m">The ResultViewModel containing the data to insert</param>

        [HttpPut("{ResultrViewModel}")]
        public IActionResult Put(ResultViewModel m)
        {
            throw new NotImplementedException();

        } // end Put

        /// <summary>
        /// Edit the Result with the given {id}
        /// </summary>
        /// <param name="m">The ResultViewModel containing the data to update</param>
        [HttpPost("{ResultViewModel}")]
        public IActionResult Post(AnswerViewModel m)
        {
            throw new NotImplementedException();

        } // end Post

        /// <summary>
        /// Deletes the Result with the given {id} from the Database
        /// </summary>
        /// <param name="id">The ID of an existing Result</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();

        } // end Delete

        #endregion 



        #region Attribute-based routing methods
        // GET: api/result/all
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {

            var sampleResults = new List<ResultViewModel>();

            // add a first sample question
            sampleResults.Add(new ResultViewModel()
            {
                Id = 1,
                QuizId = quizId,
                Text = "What is the meaning of life?",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now

            });


            // add a bunch of other sample results
            for (int i = 2; i <= 5; i++)
            {
                sampleResults.Add(new ResultViewModel()
                {
                    Id = i,
                    QuizId = quizId,
                    Text = $"Sample Question {i}",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            } // end for loop


            return new JsonResult(
                sampleResults,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                }
            ); // end return

        } // end All

        #endregion
    } // end class
}
