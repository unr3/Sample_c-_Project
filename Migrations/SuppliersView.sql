IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_Suppliers]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[V_Suppliers]
AS
select  ADAMAR17Y.dbo.TRK(CARI_ISIM) as VesselName, cari_kod as Id,EMAIL as MAIL from ADAMAR17Y..tblcasabit
where CARI_KOD like ''320%''
'