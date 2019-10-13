using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestMakerFreeWebApp.ViewModels;
using Newtonsoft.Json;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {

        #region RESTful conventions method
        /// <summary>
        /// GET: api/question/{id}
        /// Retrieves the Question with the given {id}
        /// </summary>
        /// <param name="id">The ID of an existing Question</param>
        /// <returns>the Question with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Content("Not implemented (yet)!");

        } // end Get

        /// <summary>
        /// Adds a new Question to the Database
        /// </summary>
        /// <param name="m">The QuestionViewModel containing the data to insert</param>

        [HttpPut("{QuestionViewModel}")]
        public IActionResult Put(QuestionViewModel m)
        {
            throw new NotImplementedException();

        } // end Put

        /// <summary>
        /// Edit the Question with the given {id}
        /// </summary>
        /// <param name="m">The QuestionViewModel containing the data to update</param>
        [HttpPost("{QuestionViewModel}")]
        public IActionResult Post(QuestionViewModel m)
        {
            throw new NotImplementedException();

        } // end Post

        /// <summary>
        /// Deletes the Question with the given {id} from the Database
        /// </summary>
        /// <param name="id">The ID of an existing Question</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();

        } // end Delete

        #endregion 

        #region Attribute-based routing methods
        // GET: api/question/all
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {

            var sampleQuestions = new List<QuestionViewModel>();

            // add a first sample question
            sampleQuestions.Add(new QuestionViewModel()
            {
                Id = 1,
                QuizId = quizId,
                Text = "What is the meaning of life?",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now

            });


            // add a bunch of other sample questions
            for(int i=2; i<=5; i++)
            {
                sampleQuestions.Add(new QuestionViewModel()
                {
                    Id = i,
                    QuizId = quizId,
                    Text = $"Sample Question {i}",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                }) ;
            } // end for loop


            return new JsonResult(
                sampleQuestions,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                }
            ); // end return

        } // end All
        #endregion
    } // end class
}
