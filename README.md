###NPoco.T4

NPoco.T4 is a port for NPoco of the T4 templates from PetaPoco to generate POCO models from the database and assorted other helpful classes.  The original was written by Topten Software and was based on the SubSonic T4 templates.  NPoco is the wonderful branch from Schotime of the PetaPoco microORM.

*Note – NPoco.T4 currently only works with SQL Server.  All of the hooks are still in place for the other NPoco supported DB types, so adding them back in should not be difficult.*

####What It Does

* Generates one model for each table and view in the DB.  Individual tables/views can be excluded or it can be set to exclude all and you can include individual tables/views on an opt-in basis.

* Generates generic repository class scaffolds for each model.

* Generates a base class for the models enabling basic CRUD operations directly on the POCOs.

* Allows you to designate specific tables/views as holding enum values and generates enum classes for them along with a prepopulated list class and ASP.NET MVC SelectList.

* Creates a single TypeScript definitions file with interfaces for each of the POCOs if you want to pass them to JavaScript.  (Property names are camel cased.)

####How It's Organized

NPoco.T4 is setup as an example project using NPoco as the ORM.  All of the templates are located under the NPoco_T4Generator folder.  Settings can be found at the top of _NPocoT4.tt.

App.config assumes you have a local copy of SQL Server and a database called “NPoco_Tests”.  If you run the tests in NPoco.T4.Tests, it will create the database for you along with the sample tables.

Models and repositories are deposited in the "Generated" folders under (you guessed it), Models and Repositories.  Models/Core contains a sample custom type to illustrate NPoco’s custom type mapping feature.  Repositories/Core has the custom type mapper which is baked into the repository templates.  The interfaces here allow automated CRUD testing of all of the read / write repos.


####Keep In Mind
* In your own project, manage the settings for overwrite POCOs and overwrite repos.  They default to "true" and will  blow out any customization you do.

* Comments and contributions are very welcome. 


