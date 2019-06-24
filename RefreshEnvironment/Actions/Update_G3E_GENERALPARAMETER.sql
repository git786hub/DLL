spool Update_G3E_GENERALPARAMETER_&SDATE..log

DECLARE

   	PARAM_G3E_VALUE VARCHAR2(10);
    
BEGIN
    PARAM_G3E_VALUE := 'ONCOR&ENV_REFRESH_TARGET';

    --Specifies the G3E_DATACONNECTION.G3E_LOCATION that is referenced from G3E_CONFIGURATIONS that contains all of the regions in a subset database. If you have not implemented regional subsetting, this value should be the name of the production data connection.
    update GIS.g3e_generalparameter set g3e_value=PARAM_G3E_VALUE where g3e_name='AllConfigurations'; 

    --Defines a UNC (Universal Naming Convention) path to the directory on the database server defined in the DatabaseLocalDirectory parameter.  The specified folder must be shared. If the folder is not shared, even though the folder might be on the same system on which you are running G/Technology, you get a path/file access error.  When your database resides on a UNIX operating system, the DatabaseUNCDirectory allows UNIX machines to share drives through the UNC conventions. See Publishing Metadata for UNIX for further information.
    update GIS.g3e_generalparameter set g3e_value='DATABASEUNCDIRECTORY' where g3e_name='DatabaseUNCDirectory'; 

    --References an Oracle directory, G3E_DATABASELOCALDIRECTORY pointing to a physical folder on the database host system. Used by G/Technology database packages to write files to.
    update GIS.g3e_generalparameter set g3e_value='DatabaseLocalDirectory' where g3e_name='DatabaseLocalDirectory'; 

    commit;

end;
/
