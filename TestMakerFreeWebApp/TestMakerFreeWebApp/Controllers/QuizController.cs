using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestMakerFreeWebApp.ViewModels;
using Newtonsoft.Json;
using TestMakerFreeWebApp.Data;
using Mapster;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
        #region Private Fields
        private ApplicationDbContext DbContext;
        #endregion

        #region Constructor
        public QuizController(ApplicationDbContext context)
        {
            // Instantiate the ApplicationDbContext through DI
            DbContext = context;
        }
        #endregion
        #region RESTful conventions method
        /// <summary>
        /// GET: api/quiz/{id}
        /// Retrieves the Quiz  with the given {id}
        /// </summary>
        /// <param name="id">The ID of an existing Quiz</param>
        /// <returns>the Quiz with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // create a sample quiz to match the given response
            var quiz = DbContext.Quizzes.Where(q => q.Id == id).FirstOrDefault();

            // handle requests asking for non-existing quizzes
            if (quiz == null)
            {

                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found", id)
                }); 

            } // end if

            // output the result in JSON format
            return new JsonResult(
                quiz.Adapt<QuizViewModel>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                }
            ); // end return

        } // end Get



        /// <summary>
        /// Adds a new Quiz to the Database
        /// </summary>
        /// <param name="m">The QuizViewModel containing the data to insert</param>

        [HttpPost("{QuizViewModel}")]
        public IActionResult Post([FromBody]QuizViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null)
            {
                return new StatusCodeResult(500);
            } // end if

            // handle the insert (without object-mapping)
            var quiz = new Quiz();

            // properties taken from the request
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;

            // properties set from server-side
            quiz.CreatedDate = DateTime.Now;
            quiz.LastModifiedDate = quiz.CreatedDate;

            // Set a temporary author using the Admin user's userId
            // as user login isn't supported yet: we'll change this later on.
            quiz.UserId = DbContext.Users
                            .Where(u => u.UserName == "Admin")
                            .FirstOrDefault().Id;

            // add the new quiz
            DbContext.Quizzes.Add(quiz);

            // persist the changes into the Database
            DbContext.SaveChanges();

            // return the newly-created Quiz to the client.
            return new JsonResult(quiz.Adapt<QuizViewModel>(),

                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented

                });
                            

        } // end Put

        /// <summary>
        /// Edit the Quiz with the given {id}
        /// </summary>
        /// <param name="m">The QuizViewModel containing the data to update</param>
        [HttpPut("{QuizViewModel}")]
        public IActionResult Put([FromBody]QuizViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null)
            {
                return new StatusCodeResult(500);
            } // end if

            // retrieve the quiz to edit
            var quiz = DbContext.Quizzes
                    .Where(q => q.Id == model.Id)
                    .FirstOrDefault();

            // handle requests asking for non-existing quizzes
            if(quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found", model.Id)
                });

            } // end if

            // handle the update (without object-mapping)
            //  by manually assigning the properties
            //  we want to accept from the request
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;

            // properties set from server-side          
            quiz.LastModifiedDate = quiz.CreatedDate;

            // persist the changes into the Database
            DbContext.SaveChanges();

            // return the newly-created Quiz to the client.
            return new JsonResult(quiz.Adapt<QuizViewModel>(),

                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented

                });

        } // end Post

        /// <summary>
        /// Deletes the Quiz with the given {id} from the Database
        /// </summary>
        /// <param name="id">The ID of an existing Quiz</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // retrieve the quiz from the Database
            var quiz = DbContext.Quizzes
                        .Where(q => q.Id == id)
                        .FirstOrDefault();

            // handle requests asking for non-existing quizzes
            if(quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found", id)
                });
            } // end if

            // remove the quiz from DbContext.
            DbContext.Quizzes.Remove(quiz);

            // persist the changes into the Database
            DbContext.SaveChanges();

            // return an HTTP Status 200 (OK).
            return new OkResult();

        } // end Delete

        #endregion

        #region Attribute-based routing methods
        /// <summary>
        /// GET: api/quiz/latest
        /// Retrieves the {num} latest Quizzess
        /// </summary>
        /// <param name="num">the number of quizzes ot retrieve</param>
        /// <returns>the {num} latest Quizzes</returns>
        // 
        [HttpGet("Latest/{num:int?}")]
        public IActionResult Latest(int num=10) 
        {
            var latest = DbContext
                    .Quizzes
                    .OrderByDescending(q => q.CreatedDate)
                    .Take(num)
                    .ToArray();

            // output the result in JSON format
            return new JsonResult(
                latest.Adapt<QuizViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                }
            ); // end return

        } // end Latest

        /// <summary>
        /// Get: api/quiz/ByTitle
        /// Retrieves the {num} Quizzes sorted by Title (A to Z)
        /// </summary>
        /// <param name="num">the number of quizzes to retrieve</param>
        /// <returns>{num} Quizzes sorted by Title</returns>
        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num = 10)
        {

            var byTitle = DbContext
                .Quizzes
                .OrderBy(q => q.Title)
                .Take(num)
                .ToArray();
       
            // output the result in JSON format
            return new JsonResult(
                byTitle.Adapt<QuizViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                }
            ); // end return

        } // end ByTitle

        /// <summary>
        /// Get: api/quiz/mostViewed
        /// Retrieves the {num} random Quizzes
        /// </summary>
        /// <param name="num">the number of quizzes to retrieve</param>
        /// <returns>{num} random Quizzes</returns>
        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10)
        {
            var random = DbContext
                    .Quizzes
                    .OrderBy(q => Guid.NewGuid())
                    .Take(num)
                    .ToArray();

            // output the result in JSON format
            return new JsonResult(
                random.Adapt<QuizViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                }
            ); // end return

        } // end ByTitle

        #endregion
    } // end class
}
