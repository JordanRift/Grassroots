Elmah.MySql
==========
MySql provider for ELMAH (Error Logging Modules And Handlers)

Getting Started
----------------------

### MySQL script

You will first of all need to execute the MySql script which can be found [here](http://github.com/prabirshrestha/ELMAH.MySql/blob/master/src/Elmah.MySql/MySql.sql)
Incase you download from the downloads tab, execute the mysql.sql script that comes with the binary distribution.

### Web.Config

You will then need to add the errorLog type

        <errorLog type="Elmah.MySqlErrorLog, Elmah.MySql" connectionStringName="YourConnectionStringName" />

## Download the latest binaries

You can download the latest binaries at [http://github.com/prabirshrestha/ELMAH.MySql/downloads](http://github.com/prabirshrestha/ELMAH.MySql/downloads)

## License

Elmah.MySql is intended to be used in both open-source and commercial environments. It is licensed under MIT license. (This license doesn't apply for MySql.Data.dll). Please review LICENSE.txt for more details.

#### Notes

If you are new to ELMAH refer to this [tutorial](http://dotnetslackers.com/articles/aspnet/ErrorLoggingModulesAndHandlers.aspx) on getting started with ELMAH. 


[Prabir Shrestha](http://www.prabir.me)

Follow me on twitter [@prabirshrestha](http://www.twitter.com/prabirshrestha)