The main objective of this project is to provide the hand free access to the library portal through web
interface. This project gives the complete information about the library. We
can enter the record of new books and retrieve the details of books available in the library. We can issue
the books to the clients and maintain their records and can also check how many books are issued and
stock available in the library. In this project we can maintain the late fine of clients who returns the
issued books after the due date.
the project has three parts database, server side and client side.

Database:
the database has 6 tabels (Books, Users, Geners, Authours and Borrows).
Authours and Geners has One-to-many relationship with Books.
Users and Books has Many-to-many relationship that conenects with Borrows.

Server Side:
the server side is an API that contains 5 dlls (Dto,Models,Repositories,Uow,Validator).

Dto:
the Dto has data transfer objects (DTO) of the Entites of the project.
DTO is an object that carries data between processes.
this technique  facilitate communication between two systems (like an API and your client side) without potentially exposing sensitive information.

Models:
the Models has all the entites of project LibraryContext,Book,Author,User,Borrow,Gener which represent the database  and ServiceResponde that represents the responde.

Repositories:
the Repositories has all the logic behind the controlers.

Uow:
Uow class based on an desgin pattern which includes all the repositories instances and instead of making dependecy injection to each repository
you will just need to do dependecy injection to Uow class to have access to all repositories.

Validator:
Validator has all the input validation logic of the data that received from the client side.


the api has 6 controllers:
auth which represent the auth operestions like Login and Registration and books,users,geners,authors which represents
the crud oporetions.
all the controllers use the logic form the Repositories.

books and user controllers have an option to add images.
when you add img you should make sure the name of the img is like the id of the Book/User.
you have two ways to add the images:
one is with a request and the other one is by putting the img inside wwwroot\Uploads\Books or wwwroot\Uploads\Users dependse on the img topic.
the other on is manually by creating a folder by the user/book id and the name should be image.png.
all the controllers are Authorize with  some functions that are role based and some are open to all.

client side:
the windows that are in the client side:
Home section has the welcome of the user and explains in 3 steps how to borrow with a redirection to the books.
and shows 3 books.

Books section has a search bar which enable you to filter the books.
all the books shows in a card which has two buttons  info and borrow.
the borrow button will open a modal which let you to borrow the book and if you want to see info about the book before
the info button that will open a modal which contains the info about the book and borrow option.
in order to borrow to borrow books you must to login first (if you dont own user you should register).
after you will loged you be able to borrow  books.

Borrows section has a table which contains the borrows you made in order to return the book you will be need to click the return botton.
this table has a filter and sort.
when the borrow has 1 day left till return the row will be red.
about us section has the informetion of  the web.
 
404 page that will apear if you try to access a page the dont exsit.

401 page that will be show if you try to enter a page that you dont have the access to.

Crud pages:

there are 5 crud pages Authours, Geners, Books, Users and Borrows.
all of this pages has an table that includes the recordes of the page subject, filter and sort.
you will be able to add new records by clicking on add botton which will open an modal form.
update the record  by clicking on update botton which will open an modal form.
delete the record by clicking on delete.
in Borrow will have another option which is return to return the books.

Login section whch includes login form.

Regestretion section which includes regestration form.

the client side has 3 versions the admin, client and unregistered user.

unregistered user can accses only the home, books, about-us, login and regestretion.
client user can accses the home, books, about-us, his borrows, login and regestretion.
admin user can accses all the pages.

הערה לבודק!!!!
הסיסמאות שבתוך העתק מסד הנתונים.
Admin pass:
userNameRonaldo
password g

Client Pass:
Cr7Siui
Cr7Cr7Cr7
מוגש עותק של מסד הנתונים אך במידה ואתה לא רוצב לשחזר.
כך תוכל לגשת לאדמין
ברגע ההרשמה המשתמש נרשם כלקוח לכן על מנת שתוכל לבדוק את צד האדמין של האתר ערכתי את פונקצייתה ההרשמה כך שברגע ששם המשתמש הוא
"Admin"
המשתמש ירשם כאדמין זה נועד אך ורק בשביל שתוכל להחזיק במשתמש אדמין לשם הבדיקה.












.