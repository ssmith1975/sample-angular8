import { Component, Inject, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
//import { HttpClient, HttpHeaders } from "@angular/common/http";
import { HttpClient } from "@angular/common/http";
@Component({
    selector: "quiz-edit",
    templateUrl: './quiz-edit.component.html',
    styleUrls: ['./quiz-edit.component.css']
})

export class QuizEditComponent {
    title: string;
    quiz: Quiz;

    // this will be TRUE when editing an existing quiz,
    // FALSE when creating a new one.
    editMode: boolean;


  

    constructor(private activatedRoute: ActivatedRoute,
        private router: Router,
        private http: HttpClient,
        @Inject('BASE_URL') private baseUrl: string) {

        // create an empty object from the Quiz interface
        this.quiz = <Quiz>{};
        this.title = "";

        var id = +this.activatedRoute.snapshot.params["id"];

        if (id) {
            this.editMode = true;

            // fetch the quiz from the server
            var url = this.baseUrl + "api/quiz/" + id;
            console.log(url);

            try {
                this.http.get<Quiz>(url).subscribe(res => {
                    this.quiz = res;
                    this.title = "Edit - " + this.quiz.Title;
                }, error => console.error(error));
            } catch (ex) {
                console.log(ex);
            }
        } else {
            
            this.editMode = false;
            this.title = "Create a new Quiz";

        }// end if



    } // end constructor

    onSubmit(quiz: Quiz) {
        var url = this.baseUrl + "api/quiz";
        
        if (this.editMode) {
            try {
                this.http
                    .post<Quiz>(url, quiz /*, _headers { headers: _headers, responseType: 'text' as 'json'}*/)
                    .subscribe(res => {
                        var v = res;
                        console.log("Response from post request....");
                        console.log("Quiz " + v.Id + " has been updated.");
                        console.log(v);
                        this.router.navigate(["home"]);
                    }, error => console.log(error));

                console.log('post request complete');
            } catch (ex) {
                console.log(ex);
            }
        } else {
            try {


                this.http
                    .put<Quiz>(url, quiz /*,  _headers*/)
                    .subscribe(res => {
                        var q = res;
                        console.log('Response from put request...');
                        console.log("Quiz " + q.Id + " has been created");
                        console.log(q);
                        this.router.navigate(["home"]);
                    }, error => console.log(error));
                console.log('put request completed');
            } catch (ex) {
                console.log(ex);
            }

        } // end if
    } // end onSubmit


    onBack() {
        this.router.navigate(["home"]);

    } // end onBack
}