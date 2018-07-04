IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_Vessels]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[V_Vessels]
AS
select  ADAMAR17Y.dbo.TRK(CARI_ISIM) as VesselName, cari_kod as Id from ADAMAR17Y..tblcasabit
where CARI_KOD like ''120%''
'