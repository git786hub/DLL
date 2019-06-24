DROP PACKAGE BODY GET_ARB_NOTES_PKG;

CREATE OR REPLACE PACKAGE BODY         "GET_ARB_NOTES_PKG" 
AS
   PROCEDURE CREATE_TEMP_TABLE
   AS
      sqlstr        VARCHAR2 (2048);
      column_name   VARCHAR2 (80);
      rowcnt        INTEGER;

   BEGIN

      sqlstr := 'CREATE TABLE GT_PLOT_TMP_MULTI_TEXT (NOTES VARCHAR2 (2048 Byte))';

      EXECUTE IMMEDIATE sqlstr;
   END CREATE_TEMP_TABLE;

   PROCEDURE DROP_TEMP_TABLE
   AS
      sqlstr   VARCHAR2 (2048);
   BEGIN
      EXECUTE IMMEDIATE 'DROP TABLE GT_PLOT_TMP_MULTI_TEXT';
   END DROP_TEMP_TABLE;

   PROCEDURE POP_ARB_NOTES_TEMP_TABLE (vJob_ID VARCHAR2, vPlan_Num VARCHAR2, bDetail Number)
   AS

      vNotes                  VARCHAR2 (2048);

      sqlstr     VARCHAR2 (2048);
      sqlInsert  VARCHAR2 (2048);

      TYPE newcursor IS REF CURSOR;

      v_cursor   newcursor;

   BEGIN

      EXECUTE IMMEDIATE 'DELETE GT_PLOT_TMP_MULTI_TEXT';

      IF (bDetail=0)
      THEN
         sqlstr := 'SELECT ''ARB ''||A.ARB_NUMBER||'': ''|| REPLACE(A.NOTES, chr(10),chr(10)||SUBSTR(''                    '', 1, Length(A.ARB_NUMBER)*2+9)) as NOTES FROM GC_ARB A, GC_NETELEM N WHERE A.G3E_FID=N.G3E_FID AND A.NOTES is not NULL AND N.JOB_ID=('''||vJob_ID||''') AND A.G3E_FID in (
            SELECT S.G3E_FID FROM GC_WRKBND_P P, GC_ARB_S S WHERE  P.G3E_FID in (SELECT W.G3E_FID FROM GC_WRKBND W, GC_NETELEM N WHERE W.PLAN_ID='''||vPlan_Num||''' and W.G3E_FID=N.G3E_FID  AND N.JOB_ID=('''||vJob_ID||'''))
              AND
              SDO_RELATE(S.G3E_GEOMETRY, P.G3E_GEOMETRY, ''mask=ANYINTERACT querytype = WINDOW'') = ''TRUE'' ) order by ARB_NUMBER';

         --sqlstr := 'SELECT A.NOTES FROM B$GC_ARB A, B$GC_NETELEM N WHERE A.G3E_FID=N.G3E_FID AND A.NOTES is not NULL AND N.JOB_ID=('''||vJob_ID||''')';

      ELSE
         sqlstr := 'SELECT ''ARB ''||A.ARB_NUMBER||'': ''|| REPLACE(A.NOTES, chr(10),chr(10)||SUBSTR(''                    '', 1, Length(A.ARB_NUMBER)*2+9)) as NOTES FROM GC_ARB A, GC_NETELEM N WHERE A.G3E_FID=N.G3E_FID AND A.NOTES is not NULL AND N.JOB_ID=('''||vJob_ID||''') AND A.G3E_FID in (
            SELECT S.G3E_FID FROM DGC_WRKBND_P P, DGC_ARB_S S WHERE  P.G3E_FID in (SELECT W.G3E_FID FROM GC_WRKBND W, GC_NETELEM N WHERE W.PLAN_ID='''||vPlan_Num||''' and W.G3E_FID=N.G3E_FID  AND N.JOB_ID=('''||vJob_ID||'''))
              AND
              SDO_RELATE(S.G3E_GEOMETRY, P.G3E_GEOMETRY, ''mask=ANYINTERACT querytype = WINDOW'') = ''TRUE'' ) order by ARB_NUMBER';
      END IF;

      BEGIN
         OPEN v_cursor FOR sqlstr;
         LOOP
            FETCH v_cursor
            INTO   vNotes;

	    EXIT WHEN v_cursor%NOTFOUND;

            sqlInsert :=  'INSERT into GT_PLOT_TMP_MULTI_TEXT (NOTES) VALUES (''' || vNotes || ''')';

            EXECUTE IMMEDIATE sqlInsert;

         END LOOP;

      EXCEPTION
         WHEN NO_DATA_FOUND
         THEN
            gc_message_pkg.save_error (SUBSTR (SQLERRM, 1, 200), 0);
         WHEN TOO_MANY_ROWS
         THEN
            gc_message_pkg.save_error (SUBSTR (SQLERRM, 1, 200), 0);
         WHEN OTHERS
         THEN
            gc_message_pkg.save_error (SUBSTR (SQLERRM, 1, 200), 0);
      END;


      CLOSE v_cursor;

   END POP_ARB_NOTES_TEMP_TABLE;

END GET_ARB_NOTES_PKG;

/

CREATE OR REPLACE PUBLIC SYNONYM GET_ARB_NOTES_PKG FOR GET_ARB_NOTES_PKG;


GRANT EXECUTE ON GET_ARB_NOTES_PKG TO PUBLIC;

GRANT EXECUTE ON GET_ARB_NOTES_PKG TO SUPERVISOR2;
