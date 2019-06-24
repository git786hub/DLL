DROP TABLE GIS_ONC.CULIB_MACROCATEGORY CASCADE CONSTRAINTS;


 CREATE TABLE GIS_ONC.CULIB_MACROCATEGORY 
   (ID NUMBER GENERATED ALWAYS AS IDENTITY, 
	MU_ID VARCHAR2(15 BYTE) NOT NULL, 
	CATEGORY_C VARCHAR2(15 BYTE) NOT NULL, 
	AUD_CREATE_USR_ID VARCHAR2(30 BYTE) DEFAULT USER, 
	AUD_MOD_USR_ID VARCHAR2(30 BYTE), 
	AUD_CREATE_DT TIMESTAMP (6) DEFAULT SYSDATE, 
	AUD_MOD_DT TIMESTAMP (6));
  
ALTER TABLE GIS_ONC.CULIB_MACROCATEGORY ADD CONSTRAINT CULIB_MACROCATEGORY_PK PRIMARY KEY (MU_ID, CATEGORY_C);

GRANT INSERT, UPDATE, DELETE, SELECT ON GIS_ONC.CULIB_MACROCATEGORY TO GIS_INFA;