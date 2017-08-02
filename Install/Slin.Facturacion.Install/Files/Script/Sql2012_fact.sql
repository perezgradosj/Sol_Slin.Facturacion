/****** Object:  Schema [Conf]    Script Date: 10/04/2017 18:56:47 ******/
CREATE SCHEMA [Conf]
GO
/****** Object:  Schema [Ctl]    Script Date: 10/04/2017 18:56:47 ******/
CREATE SCHEMA [Ctl]
GO
/****** Object:  Schema [Fact]    Script Date: 10/04/2017 18:56:47 ******/
CREATE SCHEMA [Fact]
GO
/****** Object:  Schema [Mtro]    Script Date: 10/04/2017 18:56:47 ******/
CREATE SCHEMA [Mtro]
GO
/****** Object:  Schema [Seg]    Script Date: 10/04/2017 18:56:47 ******/
CREATE SCHEMA [Seg]
GO
/****** Object:  StoredProcedure [Conf].[Usp_DeleteTipoDoc_Print]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_DeleteTipoDoc_Print]
@Tipo_ce VARCHAR(2),
@IdEstado INT,
@RucEntity VARCHAR(11)
AS
BEGIN
	DELETE FROM [Fact].[TipoDocumentPrint]
	WHERE TIPO_CE = @Tipo_ce AND
		  IDESTADO = @IdEstado AND
		  RucEntity = @RucEntity
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_DeleteTipoDoc_Send]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_DeleteTipoDoc_Send]
@Tipo_ce VARCHAR(2),
@IdEstado INT,
@RucEntity VARCHAR(11)
AS
BEGIN
	DELETE FROM [Fact].[TipoDocumentSend]
	WHERE TIPO_CE = @Tipo_ce AND
		  IDESTADO = @IdEstado AND
		  RucEntity = @RucEntity
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_Get_us_pwd_amb]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_Get_us_pwd_amb] --2,'20547025319'
@IdAmb INT,
@RucEntity VARCHAR(11)
AS
BEGIN
	SELECT
		RucEntity,
		NombreUsuario,
		[Password],
		IDAMBIENTE
	FROM [Fact].[CredentialsCertificateAmb]
	WHERE IDAMBIENTE = @IdAmb AND RucEntity = @RucEntity
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_GetCertificateData]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [Conf].[Usp_GetCertificateData]
@RucEntity varchar(11)
AS
BEGIN
	SELECT * FROM [Conf].[CertificateDigital] WHERE RucEntity = @RucEntity
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_GetList_TimeService]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_GetList_TimeService]
AS
BEGIN
	SELECT DISTINCT(SubType), CodeService, NameService FROM [Conf].[TimeService]
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_GetListCoin]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_GetListCoin]
AS
BEGIN
	SELECT
		Id AS IdMoneda,
		Simbolo,
		(Simbolo + ' ' + Descripcion) AS Descripcion
	FROM [Fact].[Coin]
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_GetListDocAmbiente]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Conf].[Usp_GetListDocAmbiente] --2
	@IDAmbiente [int],
	@RucEntity [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		CDA.[ID],
		CDA.[ID_TIPO_CE],
		CDA.[TIPO_CE],
		CDA.[DESCR_TPO_CE],
		CDA.[IDAMBIENTE],
		ME.[IDESTADO],
		ME.[Descripcion] AS DESCRIPCION
	FROM [Mtro].[Estado] AS ME INNER JOIN
		 [Conf].[DocumentoAmbiente] AS CDA ON ME.[IdEstado] = CDA.[IDESTADO]
	WHERE CDA.IDESTADO = 1 AND
		  CDA.IDAMBIENTE = @IDAMBIENTE AND
		  CDA.RUCENTITY = @RucEntity
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_GetListURLAmbiente]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Conf].[Usp_GetListURLAmbiente]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		[ID_URL],
		[COD_URL],
		[DESCR_URL],
		[URL],
		[IDESTADO],
		[IDAMBIENTE]
	FROM [Conf].[URL_SUNAT]
END
GO
/****** Object:  StoredProcedure [Conf].[Usp_GetStatusPrint_TPO_CE]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [Conf].[Usp_GetStatusPrint_TPO_CE]
@IdEstadoDocumento INT,
@RucEntity VARCHAR(11)
AS
BEGIN
	SELECT 
		FTD.[ID] AS IdEstadoEnvio,
		FTD.[TIPO_CE] AS CodigoDocumento,
		CTD.Id_TD AS IdTipoDocumento
	FROM [Ctl].[TipoDocumento] AS CTD INNER JOIN 
	     [Fact].[TipoDocumentPrint] AS FTD ON CTD.Code = FTD.TIPO_CE
	WHERE FTD.IDESTADO = @IdEstadoDocumento AND
		  FTD.RucEntity = @RucEntity
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_GetStatusSend_TPO_CE]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_GetStatusSend_TPO_CE]
@IdEstadoDocumento INT,
@RucEntity VARCHAR(11)
AS
BEGIN
	SELECT 
		FTD.[ID] AS IdEstadoEnvio,
		FTD.[TIPO_CE] AS CodigoDocumento,
		CTD.Id_TD AS IdTipoDocumento
	FROM [Ctl].[TipoDocumento] AS CTD INNER JOIN 
	     [Fact].[TipoDocumentSend] AS FTD ON CTD.Code = FTD.TIPO_CE
	WHERE FTD.IDESTADO = @IdEstadoDocumento AND
		  FTD.RucEntity = @RucEntity
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_Insert_SecondaryUserSunat]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_Insert_SecondaryUserSunat]
@RucEntity varchar(11),
@UserName varchar(250),
@Password varchar(max),
@IdAmb INT
AS
IF NOT EXISTS (select * from [Fact].[CredentialsCertificateAmb] where RucEntity = @RucEntity and IDAMBIENTE = @IdAmb)
BEGIN
	INSERT INTO [Fact].[CredentialsCertificateAmb]
		(RucEntity, NombreUsuario, [Password], IDAMBIENTE)
		VALUES
		(@RucEntity, @UserName, @Password, @IdAmb)
END
ELSE
BEGIN
	UPDATE [Fact].[CredentialsCertificateAmb]
		SET NombreUsuario = @UserName, [Password] = @Password
		WHERE RucEntity = @RucEntity AND IDAMBIENTE = @IdAmb
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_InsertCredentialCertificateAmb]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_InsertCredentialCertificateAmb]
@RucEntity VARCHAR(11),
@NombreUsuario VARCHAR(250),
@Password VARCHAR(MAX),
@IDAMBIENTE INT
AS
BEGIN
	INSERT INTO [Fact].[CredentialsCertificateAmb]
		VALUES
		(@RucEntity,
		@NombreUsuario,
		@Password,
		@IDAMBIENTE)
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_InsertTipoDoc_Print]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_InsertTipoDoc_Print] --1,'12345678911',1
@Tipo_ce VARCHAR(2),
@IdEstado INT,
@RucEntity VARCHAR(11)
AS
IF NOT EXISTS (SELECT ID FROM [Fact].[TipoDocumentPrint] WHERE TIPO_CE = @Tipo_ce AND IDESTADO = @IdEstado AND RucEntity = @RucEntity)
	BEGIN
		INSERT INTO  [Fact].[TipoDocumentPrint]
			([TIPO_CE], [IDESTADO], [RucEntity])
			VALUES
			(@Tipo_ce, @IdEstado, @RucEntity)
	END
ELSE
	BEGIN
		UPDATE [Fact].[TipoDocumentPrint] SET [TIPO_CE] = @Tipo_ce
		WHERE TIPO_CE = @Tipo_ce AND IDESTADO = @IdEstado AND RucEntity = @RucEntity
	END

GO
/****** Object:  StoredProcedure [Conf].[Usp_InsertTipoDoc_Send]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_InsertTipoDoc_Send] --1,'12345678911',1
@Tipo_ce VARCHAR(2),
@IdEstado INT,
@RucEntity VARCHAR(11)
AS
IF NOT EXISTS (SELECT ID FROM [Fact].[TipoDocumentSend] WHERE TIPO_CE = @Tipo_ce AND IDESTADO = @IdEstado AND RucEntity = @RucEntity)
	BEGIN
		INSERT INTO  [Fact].[TipoDocumentSend]
			([TIPO_CE], [IDESTADO], [RucEntity])
			VALUES
			(@Tipo_ce, @IdEstado, @RucEntity)
	END
ELSE
	BEGIN
		UPDATE [Fact].[TipoDocumentSend] SET [TIPO_CE] = @Tipo_ce
		WHERE TIPO_CE = @Tipo_ce AND IDESTADO = @IdEstado AND RucEntity = @RucEntity
	END
GO
/****** Object:  StoredProcedure [Conf].[Usp_InsertTypeDoc_ForPrint]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_InsertTypeDoc_ForPrint] --'20',7,'20106896276'
@TypeDoc VARCHAR(2),
@IdEstado INT,
@RucEntity VARCHAR(11)
AS
--IF NOT EXISTS (SELECT ID FROM [Fact].[TipoDocumentPrint_Test] WHERE TIPO_CE = @TypeDoc AND RucEntity = @RucEntity)
IF NOT EXISTS (SELECT ID FROM [Fact].[TipoDocumentPrint] WHERE TIPO_CE = @TypeDoc AND RucEntity = @RucEntity)
	BEGIN
		--INSERT INTO  [Fact].[TipoDocumentPrint_Test]
		INSERT INTO  [Fact].[TipoDocumentPrint]
			([TIPO_CE], [IDESTADO], [RucEntity])
			VALUES
			(@TypeDoc, @IdEstado, @RucEntity)
	END
ELSE
	BEGIN
		--UPDATE [Fact].[TipoDocumentPrint_Test] SET [IDESTADO] = @IdEstado
		UPDATE [Fact].[TipoDocumentPrint] SET [IDESTADO] = @IdEstado
		WHERE TIPO_CE = @TypeDoc AND RucEntity = @RucEntity
	END

GO
/****** Object:  StoredProcedure [Conf].[Usp_InsertTypeDoc_ForSend]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_InsertTypeDoc_ForSend] --'20',7,'20106896276'
@TypeDoc VARCHAR(2),
@IdEstado INT,
@RucEntity VARCHAR(11)
AS
--IF NOT EXISTS (SELECT ID FROM [Fact].[TipoDocumentSend_Test] WHERE TIPO_CE = @TypeDoc AND RucEntity = @RucEntity)
IF NOT EXISTS (SELECT ID FROM [Fact].[TipoDocumentSend] WHERE TIPO_CE = @TypeDoc AND RucEntity = @RucEntity)
	BEGIN
		--INSERT INTO  [Fact].[TipoDocumentSend_Test]
		INSERT INTO  [Fact].[TipoDocumentSend]
			([TIPO_CE], [IDESTADO], [RucEntity])
			VALUES
			(@TypeDoc, @IdEstado, @RucEntity)
	END
ELSE
	BEGIN
		--UPDATE [Fact].[TipoDocumentSend_Test] SET [IDESTADO] = @IdEstado
		UPDATE [Fact].[TipoDocumentSend] SET [IDESTADO] = @IdEstado
		WHERE TIPO_CE = @TypeDoc AND RucEntity = @RucEntity
	END


GO
/****** Object:  StoredProcedure [Conf].[Usp_InsertTypeDocForSend]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_InsertTypeDocForSend]
@TypeDoc VARCHAR(2),
@IdEstado INT,
@RucEntity VARCHAR(11)
AS
--IF NOT EXISTS (SELECT ID FROM [Fact].[TipoDocumentSend_Test] WHERE TIPO_CE = @TypeDoc AND RucEntity = @RucEntity)
IF NOT EXISTS (SELECT ID FROM [Fact].[TipoDocumentSend_Test] WHERE TIPO_CE = @TypeDoc AND RucEntity = @RucEntity)
	BEGIN
		--INSERT INTO  [Fact].[TipoDocumentSend_Test]
		INSERT INTO  [Fact].[TipoDocumentSend]
			([TIPO_CE], [IDESTADO], [RucEntity])
			VALUES
			(@TypeDoc, @IdEstado, @RucEntity)
	END
ELSE
	BEGIN
		--UPDATE [Fact].[TipoDocumentSend_Test] SET [TIPO_CE] = @TypeDoc
		UPDATE [Fact].[TipoDocumentSend] SET [TIPO_CE] = @TypeDoc
		WHERE TIPO_CE = @TypeDoc AND IDESTADO = @IdEstado AND RucEntity = @RucEntity
	END


GO
/****** Object:  StoredProcedure [Conf].[Usp_ListTimeService]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_ListTimeService]
@RucEntity VARCHAR(11)
AS
BEGIN
	SELECT 
		CTS.Id,
		CTS.CodeService,
		CTS.NameService,
		CTS.ValueTime,
		CTS.IntervalValue,
		CTS.MaxNumAttempts,
		CTS.RucEntity,
		ME.IdEstado,
		ME.Descripcion,
		'' AS ServiceStatus
	FROM [Mtro].[Estado] AS ME INNER JOIN 
		 [Conf].[TimeService] AS CTS ON ME.IdEstado = CTS.IdEstado
	WHERE RucEntity = @RucEntity
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_ListTypeDocument_TypePrint]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Conf].[Usp_ListTypeDocument_TypePrint] --'20106896276'
@RucEntity VARCHAR(11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		CTD.Id_TD as IdTipoDocumento,
		CTD.Code AS CodigoDocumento,
		CTD.[Desc] AS Descripcion,
		CTD.[Padre],
		TDS.IDESTADO AS IdEstado,
		ED.[Desc] AS DescripcionEstado
	FROM [Ctl].[TipoDocumento] AS CTD INNER JOIN
		 --[Fact].[TipoDocumentPrint_Test] AS TDS ON CTD.Code = TDS.TIPO_CE INNER JOIN
		 [Fact].[TipoDocumentPrint] AS TDS ON CTD.Code = TDS.TIPO_CE INNER JOIN
		 [Fact].[O.EstadoDocumento] AS ED ON TDS.IDESTADO = ED.Id_ED
	WHERE TDS.[RucEntity] = @RucEntity AND TDS.[TIPO_CE] != '07' AND TDS.[TIPO_CE] != '08' AND TDS.[TIPO_CE] != '12' AND TDS.[TIPO_CE] != '18' 
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_ListTypeDocument_TypeSend]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Conf].[Usp_ListTypeDocument_TypeSend] --'20106896276'
@RucEntity VARCHAR(11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		CTD.Id_TD as IdTipoDocumento,
		CTD.Code AS CodigoDocumento,
		CTD.[Desc] AS Descripcion,
		CTD.[Padre],
		TDS.IDESTADO AS IdEstado,
		ED.[Desc] AS DescripcionEstado
	FROM [Ctl].[TipoDocumento] AS CTD INNER JOIN
		 --[Fact].[TipoDocumentSend_Test] AS TDS ON CTD.Code = TDS.TIPO_CE INNER JOIN
		 [Fact].[TipoDocumentSend] AS TDS ON CTD.Code = TDS.TIPO_CE INNER JOIN
		 [Fact].[O.EstadoDocumento] AS ED ON TDS.IDESTADO = ED.Id_ED
	WHERE TDS.[RucEntity] = @RucEntity AND TDS.[TIPO_CE] != '07' AND TDS.[TIPO_CE] != '08' AND TDS.[TIPO_CE] != '12' AND TDS.[TIPO_CE] != '18' 
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_ObtieneIntervaloEnvio]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Conf].[Usp_ObtieneIntervaloEnvio]-- 'ADE.Services.SummaryDelivery'
AS
SET NOCOUNT ON
BEGIN
  SELECT TOP 1 IntervalValue
  FROM Conf.TimeService
  WHERE CodeService = 'ADE.Services.SunatDelivery'
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_ObtieneIntervaloServicio]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Conf].[Usp_ObtieneIntervaloServicio]-- 'ADE.Services.SummaryDelivery'
   @CodeService  AS VARCHAR(50)
--   @RucEntity    AS VARCHAR(11)
AS
SET NOCOUNT ON
BEGIN
   SELECT-- TOP 1
      Id,
      CodeService,
      NameService,
      ValueTime,
      IntervalValue,
      MaxNumAttempts,
      RucEntity,
      IdEstado,
      SubType
   FROM [Conf].[TimeService]
   WHERE 
	   CodeService = @CodeService --AND
--      RucEntity = @RucEntity
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_Register_TimeServiceCompany]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_Register_TimeServiceCompany]
@CodeService varchar(50),
@NameService varchar(150),
@ValueTime varchar(5),
@IntervalValue varchar(5),
@MaxNumAttempts int,
@RucEntity varchar(11),
@IdEstado int,
@SubType varchar(5)
AS
BEGIN
	INSERT INTO [Conf].[TimeService]
	(CodeService,NameService
	,ValueTime,IntervalValue,MaxNumAttempts
	,RucEntity,IdEstado,SubType)
	VALUES
	(@CodeService,@NameService
	,@ValueTime,@IntervalValue,@MaxNumAttempts
	,@RucEntity,@IdEstado,@SubType)
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_RegisterCertificateInformation]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_RegisterCertificateInformation]
@NameCertificate varchar(MAX), 
@Pwd varchar(max),
@ExpirationDate varchar(10),
@RucEntity varchar(11)
AS
BEGIN
	INSERT INTO [Conf].[CertificateDigital]
	(NameCertificate, Pwd, ExpirationDate, RucEntity)
	VALUES
	(@NameCertificate, @Pwd, @ExpirationDate, @RucEntity)
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_UpdateDocAmb]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Conf].[Usp_UpdateDocAmb] --2,'03',1
	@IDAMBIENTE [int],
	@TIPO_CE [varchar](2),
	@IDESTADO [int],
	@RUCENTITY [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	UPDATE [Conf].[DocumentoAmbiente] SET 
		[IDESTADO] = @IDESTADO
	WHERE --[ID] = @ID_DOC_AMB AND
		  [IDAMBIENTE] = @IDAMBIENTE AND
		  [TIPO_CE] = @TIPO_CE AND
		  [RUCENTITY] = @RUCENTITY
END

IF(@IDESTADO = 1)
BEGIN
	UPDATE [Conf].[DocumentoAmbiente]
		SET [IDESTADO] = 2
	WHERE [IDAMBIENTE] != @IDAMBIENTE AND
		  [TIPO_CE] = @TIPO_CE AND
		  [RUCENTITY] = @RUCENTITY
END

GO
/****** Object:  StoredProcedure [Conf].[Usp_UpdateTimeService]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Conf].[Usp_UpdateTimeService]
@Id INT,
@NameService VARCHAR(100),
@ValueTime VARCHAR(5),
@IntervalValue VARCHAR(5),
@MaxNumAttempts INT,
@RucEntity VARCHAR(11),
@IdEstado INT
AS
BEGIN
	UPDATE [Conf].[TimeService]
		SET ValueTime = @ValueTime,
			IntervalValue = @IntervalValue,
			MaxNumAttempts = @MaxNumAttempts,
			IdEstado = @IdEstado
	WHERE Id = @Id AND NameService = @NameService AND RucEntity = @RucEntity
END
GO
/****** Object:  StoredProcedure [Conf].[Usp_UpdateURL]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Conf].[Usp_UpdateURL]
	@ID_URL [int],
	@URL [varchar](max)
WITH EXECUTE AS CALLER
AS
BEGIN
	UPDATE [Conf].[URL_SUNAT] SET
		  [URL] = @URL--,
		  --[IDESTADO] = @IDESTADO
	WHERE ID_URL = @ID_URL --AND
		  --COD_URL = @COD_URL
END

GO
/****** Object:  StoredProcedure [Ctl].[Usp_ListaTipoDocumento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Ctl].[Usp_ListaTipoDocumento]
WITH EXECUTE AS CALLER
AS
BEGIN
	SET NOCOUNT ON
	SELECT 0 AS IdTipoDocumento
		,'' AS CodigoDocumento,
		'- Todos - ' AS Descripcion,
		0 AS Padre
	UNION
	SELECT 
		Id_TD,
		Code AS Codigo,
		[Desc] AS Descripcion,
		[Padre]
	FROM [Ctl].[TipoDocumento]
END

GO
/****** Object:  StoredProcedure [Ctl].[Usp_ListaTipoDocumentoIdentidad]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Ctl].[Usp_ListaTipoDocumentoIdentidad]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		[Id_TDI] AS IdTipoDocumento
		,[Code] AS Codigo
		,[Desc] AS Descripcion
	FROM [Ctl].[TipoDocumentoIdentidad]
END

GO
/****** Object:  StoredProcedure [Fact].[I.Usp_GetListDocumentoCabeceraReceived]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[I.Usp_GetListDocumentoCabeceraReceived] --'','',0,'','0','0',0,'','',''
@FechaInicio VARCHAR(10),
@FechaFin VARCHAR(10),
@TipoDocumento INT,
--@Serie VARCHAR(4),
@NumeroDocumentoInicio VARCHAR(20),
@NumeroDocumentoFin VARCHAR(20),
--@Estado INT,
@RucEmisor VARCHAR(20),
@RazonSocialEmisor VARCHAR(100),
--@RucEmpresaEmisor VARCHAR(11),
@RucCliente VARCHAR(11)
--@Id_ED_DOC INT
AS
BEGIN
	SELECT 
		CONVERT(INT, ROW_NUMBER() OVER(ORDER BY DC.Id_DC)) AS Nro,
		CTP.Code AS 'CodigoDocumento',
		[CTP].[Desc] AS Descripcion,
		CONVERT(VARCHAR(4),SUBSTRING(DC.ID_CPE,1,4)) AS Serie,
		DC.NUM_CPE AS NumeroDocumento,
		DC.NUM_DOCUMENTO AS NumeroFactura,
		DC.FEC_EMIS AS FechaEmision,
		DC.[EM_NUM_DOCU] AS NumDocEmpresa,
		RTRIM(DC.[EM_NOMBRE]) AS Empresa,
		DC.[EM_DFISCAL] AS EmisorDireccion,
		DC.TOT_IMPOR_TOTAL AS MontoTotal,
		CASE DC.TPO_MONEDA 
		WHEN 'PEN' THEN 'Soles'
		WHEN 'USD' THEN 'Dólares' 
		END AS Moneda,
		CTP.Id_TD AS IdTipoDocumento,
		ISNULL(DC.VAR_FIR, 0) AS 'XML'
	FROM [Fact].[I.DocumentoCabecera] AS DC LEFT JOIN
		 [Ctl].[TipoDocumento] AS CTP ON DC.ID_TPO_CPE = CTP.Code LEFT JOIN
		 [Fact].[O.EstadoDocumento] AS FED ON DC.Id_ED = FED.Id_ED LEFT JOIN 
		 [Mtro].[Distrito] AS MD ON DC.EM_UBIGEO = MD.CodigoUbigeo LEFT JOIN
		 [Fact].[O.NotasRespuesta] AS NR ON DC.NUM_CPE = NR.NUM_CPE
	WHERE
		(@FechaInicio = '' AND @FechaFin = '' OR DC.FEC_EMIS BETWEEN @FechaInicio AND @FechaFin) AND
		(@TipoDocumento = 0 OR CTP.Id_TD = @TipoDocumento) AND
		--(@Serie = '' OR DC.SERIE = @Serie) AND
		((@NumeroDocumentoInicio = '' AND @NumeroDocumentoFin = '') OR DC.NUM_DOCUMENTO BETWEEN @NumeroDocumentoInicio AND @NumeroDocumentoFin) AND
		--(@Estado = 0 OR DC.Id_ED = @Estado) AND
		(@RucEmisor = '' OR DC.[EM_NUM_DOCU] = @RucEmisor) AND
		(@RazonSocialEmisor = '' OR DC.[EM_NOMBRE] LIKE @RazonSocialEmisor + '%') AND
		(DC.[RE_NUMDOC] = @RucCliente) AND
		(CTP.Code = '01' OR CTP.Code = '03' OR CTP.Code = '07' OR CTP.Code = '08')
END
GO
/****** Object:  StoredProcedure [Fact].[I.Usp_InsertaCabecera]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[I.Usp_InsertaCabecera]
@NUM_CPE [varchar](50),
@ID_TPO_CPE [varchar](2),
@ID_CPE [varchar](13),
@ID_TPO_OPERACION [varchar](2),
@FEC_EMIS [datetime],
@TPO_MONEDA [varchar](3),
@EM_TPO_DOC [varchar](1),
@EM_NUM_DOCU [varchar](11),
@EM_NOMBRE [varchar](250),
@EM_NCOMER [varchar](250),
@EM_UBIGEO [varchar](6),
@EM_DFISCAL [varchar](250),

@EM_DURBANIZ [varchar](250),
@EM_DIR_DPTO [varchar](250),
@EM_DIR_PROV [varchar](250),
@EM_DIR_DIST [varchar](250),
@EM_COD_PAIS [varchar](5),

@RE_TPODOC [varchar](1),
@RE_NUMDOC [varchar](15),
@RE_NOMBRE [varchar](250),
@RE_DIRECCION [varchar](250),
@TOT_GRAV_MTO [decimal](12, 2),
@TOT_SUMA_IGV [decimal](12, 2),
@TOT_IMPOR_TOTAL [decimal](12, 2),
@MONTOLITERAL [varchar](50),
@SERIE [varchar](4),
@NUM_DOCUMENTO [varchar](15),
@VAR_FIR [varbinary](MAX)
AS
DECLARE @distrito VARCHAR(200)
DECLARE @provincia VARCHAR(200)
DECLARE @departamento VARCHAR(200)
DECLARE @codpais VARCHAR(20)
BEGIN	
	--EXEC @distrito = [Mtro].[Usp_ObtenerDistrito] @EM_UBIGEO;
	--EXEC @provincia = [Mtro].[Usp_ObtenerProvincia] @EM_UBIGEO
	--EXEC @departamento = [Mtro].[Usp_ObtenerDepartamento] @EM_UBIGEO
	--EXEC @codpais = [Mtro].[Usp_ObtenerCodPais] @EM_UBIGEO

	exec @distrito = [Mtro].[fn_GetDistrito] @EM_UBIGEO
	exec @provincia = [Mtro].[fn_GetProvincia] @EM_UBIGEO
	exec @departamento = [Mtro].[fn_GetDepartamento] @EM_UBIGEO
	exec @codpais = [Mtro].[fn_GetCodPais] @EM_UBIGEO;
END
BEGIN
	INSERT INTO [Fact].[I.DocumentoCabecera]
		([NUM_CPE]
        ,[ID_TPO_CPE]
        ,[ID_CPE]
        ,[ID_TPO_OPERACION]
        ,[FEC_EMIS]
		,[TPO_MONEDA]
		,[EM_TPO_DOC]
		,[EM_NUM_DOCU]
		,[EM_NOMBRE]
		,[EM_NCOMER]
		,[EM_UBIGEO]
		,[EM_DFISCAL]
		,[EM_DURBANIZ]
		,[EM_DIR_DPTO]
		,[EM_DIR_PROV]
		,[EM_DIR_DIST]
		,[EM_COD_PAIS]
		,[RE_TPODOC]
		,[RE_NUMDOC]
		,[RE_NOMBRE]
		,[RE_DIRECCION]
		,[TOT_GRAV_MTO]
		,[TOT_SUMA_IGV]
		,[TOT_IMPOR_TOTAL]
		,[MONTOLITERAL]
		,[SERIE]
		,[NUM_DOCUMENTO]
		,[Id_ED]
		,[VAR_FIR])
	OUTPUT inserted.Id_DC  
	VALUES
		(@NUM_CPE
        ,@ID_TPO_CPE
        ,@ID_CPE
        ,@ID_TPO_OPERACION
        ,@FEC_EMIS
        ,@TPO_MONEDA
		,@EM_TPO_DOC
		,@EM_NUM_DOCU
		,@EM_NOMBRE
		,@EM_NCOMER
		,@EM_UBIGEO
		,@EM_DFISCAL
		,@EM_DURBANIZ
		,@EM_DIR_DPTO
		,@EM_DIR_PROV
		,@EM_DIR_DIST
		,@EM_COD_PAIS
		,@RE_TPODOC
		,@RE_NUMDOC
		,@RE_NOMBRE
		,@RE_DIRECCION
		,@TOT_GRAV_MTO
		,@TOT_SUMA_IGV
		,@TOT_IMPOR_TOTAL
		,@MONTOLITERAL
		,@SERIE
		,@NUM_DOCUMENTO
		,'1'
		,@VAR_FIR)
	END
GO
/****** Object:  StoredProcedure [Fact].[I.Usp_InsertaDetalle]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[I.Usp_InsertaDetalle]
	--@IdDocumentoCabecera int,
	@ID_DC			INT,
	@NUM_CPE		VARCHAR (50),
	@IT_NRO_ORD		VARCHAR (3),
	@IT_UND_MED		VARCHAR (3),
	@IT_CANT_ITEM	numeric(18,5),
	@IT_COD_PROD	VARCHAR (30),
	@IT_DESCRIP		VARCHAR (250),
	@IT_VAL_UNIT	DECIMAL (12, 2),
	@IT_MNT_PVTA	DECIMAL (12, 2),
	@IT_VAL_VTA		DECIMAL (12, 2),
	@IT_MTO_IGV		DECIMAL (12, 2),
	@IT_COD_AFE_IGV VARCHAR(4),
	--@IT_MTO_ISC		DECIMAL (12, 5),
	--@IT_SIS_AFE_ISC DECIMAL (12, 5),
	--@IT_DESC_MNTO	DECIMAL (12, 5),
	@SERIE			VARCHAR(4),
	@NUM_DOCUMENTO	VARCHAR(15)
AS
BEGIN
	--DECLARE @MaxID INT select max([Id_DC]) from [Fact].[I.DocumentoCabecera]
	INSERT INTO [Fact].[I.DocumentoDetalle]
			([Id_DC]
		   ,[NUM_CPE]
		   ,[IT_NRO_ORD]
		   ,[IT_UND_MED]
		   ,[IT_CANT_ITEM]
		   ,[IT_COD_PROD]
		   ,[IT_DESCRIP]
		   ,[IT_VAL_UNIT]
		   ,[IT_MNT_PVTA]
		   ,[IT_VAL_VTA]
		   ,[IT_MTO_IGV]
		   ,[IT_COD_AFE_IGV]
		   --,[IT_MTO_ISC]
		   --,[IT_SIS_AFE_ISC]
		   --,[IT_DESC_MNTO]
		   ,[SERIE]
		   ,[NUM_DOCUMENTO]
		   )
			 VALUES
		   (@ID_DC
		   ,@NUM_CPE
		   ,@IT_NRO_ORD
		   ,@IT_UND_MED
		   ,@IT_CANT_ITEM
		   ,@IT_COD_PROD
		   ,@IT_DESCRIP
		   ,@IT_VAL_UNIT
		   ,@IT_MNT_PVTA
		   ,@IT_VAL_VTA
		   ,@IT_MTO_IGV
		   --,CONVERT(DECIMAL(12,2), @IT_VAL_UNIT)
		   --,CONVERT(DECIMAL(12,2), @IT_MNT_PVTA)
		   --,CONVERT(DECIMAL(12,2), @IT_VAL_VTA)
		   --,CONVERT(DECIMAL(12,2), @IT_MTO_IGV)
		   ,@IT_COD_AFE_IGV
		   --,CONVERT(DECIMAL(12,5), @IT_MTO_ISC)
		   --,CONVERT(DECIMAL(12,5), @IT_SIS_AFE_ISC)
		   --,CONVERT(DECIMAL(12,5), @IT_DESC_MNTO)
		   ,@SERIE
		   ,@NUM_DOCUMENTO
		   )
END
GO
/****** Object:  StoredProcedure [Fact].[I.Usp_InsertaDetalle_rp]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[I.Usp_InsertaDetalle_rp]
	@ID_DC			INT,
	@NUM_CPE		VARCHAR (50),
	@IT_NRO_ORD		VARCHAR (3),
	@SERIE			VARCHAR(4),
	@NUM_DOCUMENTO	VARCHAR(15),

	@TPODOCRELAC		VARCHAR (3),
	@NUMDOCRELAC		VARCHAR(13),
	@FEMISDOCRELAC      DATETIME,
	@ITOTDOCRELAC		DECIMAL(12,2),
	@MDOCRELAC			VARCHAR(3),
	@FECMOVI			DATETIME,
	@NUMMOVI			VARCHAR(9),
	@IMPSOPERMOV		DECIMAL(12,2),
	@MONMOVI			VARCHAR(3),
	@IMPOPER			DECIMAL(12,2),
	@MONIMPOPER			VARCHAR(13),
	@FECOPER			DATETIME,
	@IMPTOTOPER			DECIMAL(12,2),
	@MONOPER			VARCHAR(3),
	@MONREFETC			VARCHAR(3),
	@MONDESTTC			VARCHAR(3),
	@FACTORTC			VARCHAR(11),
	@FECHATC			DATETIME
AS
BEGIN
	INSERT INTO [Fact].[I.DocumentoDetalle]
			([Id_DC]
		   ,[NUM_CPE]
		   ,[IT_NRO_ORD]
		   ,[SERIE]
		   ,[NUM_DOCUMENTO]

		   ,[TPODOCRELAC]
		   ,[NUMDOCRELAC]
		   ,[FEMISDOCRELAC]
		   ,[ITOTDOCRELAC]
		   ,[MDOCRELAC]
		   ,[FECMOVI]
		   ,[NUMMOVI]
		   ,[IMPSOPERMOV]
		   ,[MONMOVI]
		   ,[IMPOPER]
		   ,[MONIMPOPER]
		   ,[FECOPER]
		   ,[IMPTOTOPER]
		   ,[MONOPER]
		   ,[MONREFETC]
		   ,[MONDESTTC]
		   ,[FACTORTC]
		   ,[FECHATC]
		   )
			 VALUES
		   (@ID_DC
		   ,@NUM_CPE
		   ,@IT_NRO_ORD
		   ,@SERIE
		   ,@NUM_DOCUMENTO
		   
		   ,@TPODOCRELAC
		   ,@NUMDOCRELAC
		   ,@FEMISDOCRELAC
	       ,@ITOTDOCRELAC
		   ,@MDOCRELAC
		   ,@FECMOVI
		   ,@NUMMOVI
		   ,@IMPSOPERMOV
		   ,@MONMOVI
		   ,@IMPOPER
		   ,@MONIMPOPER
		   ,@FECOPER
		   ,@IMPTOTOPER
		   ,@MONOPER
		   ,@MONREFETC
		   ,@MONDESTTC
		   ,@FACTORTC
		   ,@FECHATC
		   )
END
GO
/****** Object:  StoredProcedure [Fact].[MailPDF]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[MailPDF]
	@Table        AS VARCHAR(1),
   @DocumentType AS VARCHAR(2),
   @State        AS VARCHAR(2),
   @Ruc          AS VARCHAR(11)
AS
   SET NOCOUNT ON;
   IF @Table = 'S'
      BEGIN
         SELECT 
            ID
         FROM 
            [Fact].[TipoDocumentSend]
         WHERE
            TIPO_CE = @DocumentType AND
            IDESTADO = @State AND
            RucEntity = @Ruc
      END
   ELSE IF @Table = 'P'
      BEGIN
         SELECT 
            ID
         FROM 
            [Fact].[TipoDocumentPrint]
         WHERE
            TIPO_CE = @DocumentType AND
            IDESTADO = @State AND
            RucEntity = @Ruc
      END

GO
/****** Object:  StoredProcedure [Fact].[O.EliminaDocumento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.EliminaDocumento] --'205086622326-01-F001-00000110'
	@NUM_CPE        [varchar](50)
AS
   SET NOCOUNT ON;
   DECLARE @ID_CE INT;
   EXEC @ID_CE = Fact.fn_GetIdCE @NUM_CPE
   BEGIN
      -- ELIMINA DOCUMENTOS AFECTADOS
      DELETE FROM 
         Fact.[O.Afectado]
      WHERE
         ID_DC = @ID_CE;

      -- ELIMINA ANTICIPOS
      DELETE FROM 
         Fact.[O.Anticipo]
      WHERE
         ID_DC = @ID_CE;

      -- ELIMINA DOCUMENTOS AFECTADOS
      DELETE FROM 
         Fact.[O.DocumentoAfectado]
      WHERE
         ID_DC = @ID_CE;

      -- ELIMINA DETALLE DOCUMENTOS
      DELETE FROM 
         Fact.[O.DocumentoDetalle]
      WHERE
         Id_DC = @ID_CE;

      -- ELIMINA DOCUMENTOS AFECTADOS
      DELETE FROM 
         Fact.[O.Extra]
      WHERE
         ID_DC = @ID_CE;

      -- ELIMINA NOTAS RESPUESTA
      DELETE FROM 
         Fact.[O.NotasRespuesta]
      WHERE
         ID_CPE = @ID_CE;

      -- ELIMINA REFERENCIAS
      DELETE FROM 
         Fact.[O.Referencia]
      WHERE
         ID_DC = @ID_CE;

      -- ELIMINA REFERENCIAS
      DELETE FROM 
         Fact.[O.Documento_Anul]
      WHERE
         (RucEmpresa + '-' + TypeDoc + '-' + Serie + '-' + Correlativo) = @NUM_CPE;

      -- ELIMINA DETALLE DOCUMENTOS
      DELETE FROM 
         Fact.[O.DocumentoCabecera]
      WHERE
         Id_DC = @ID_CE;
   END

GO
/****** Object:  StoredProcedure [Fact].[O.R.Usp_ListaCabeceraRA]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [Fact].[O.R.Usp_ListaCabeceraRA] --'2015-12-07','2015-12-12',0,2
@Fechadesde VARCHAR(10),
@Fechahasta VARCHAR(10),
@IdEstado INT,
@TipoFecha INT,
@RucEntity VARCHAR(11)
AS
	IF(@TipoFecha = 1)
	BEGIN
		SELECT
			CONVERT(INT, ROW_NUMBER() OVER(ORDER BY FRA.[ID_RAC])) AS Nro,
			FRA.[ID_RAC] AS Correlativo,
			FRA.FEC_REF AS FechaInicio,
			FRA.FEC_REF AS FechaFin,
			FRA.[FEC_CAD] AS FechaEnvio,
			CONVERT(VARCHAR,FRA.NUM_SEC) AS Secuencia,
			FED.[Id_ED] AS IdEstado,
			FED.[Desc] AS Estado,
			SUBSTRING([DOC_MSG], 0, 30) + '...'  AS MensajeEnvio,
			[DOC_MSG] AS MensajeEnvioDetalle,
			[DOC_TCK] AS NumeroAtencion,
			ISNULL([VAR_FIR], 0) AS 'XML',
			[NUM_CPE] AS NombreArchivo
		FROM [Fact].[O.RBajasCabecera] AS FRA INNER JOIN
			 [Fact].[O.EstadoDocumento] AS FED ON FRA.DOC_EST = FED.Id_ED
		WHERE (@Fechadesde = '' AND @Fechahasta = '' OR FRA.FEC_REF BETWEEN @Fechadesde AND @Fechahasta) --AND
			  AND FRA.[TIPO] = 'RA' AND FRA.RucEntity = @RucEntity
	END
	ELSE
	BEGIN
		SELECT
			CONVERT(INT, ROW_NUMBER() OVER(ORDER BY FRA.[ID_RAC])) AS Nro,
			FRA.[ID_RAC] AS Correlativo,
			FRA.FEC_REF AS FechaInicio,
			FRA.FEC_REF AS FechaFin,
			FRA.[FEC_CAD] AS FechaEnvio,
			CONVERT(VARCHAR,FRA.NUM_SEC) AS Secuencia,
			FED.[Id_ED] AS IdEstado,
			FED.[Desc] AS Estado,
			SUBSTRING([DOC_MSG], 0, 30) + '...' AS MensajeEnvio,
			[DOC_MSG] AS MensajeEnvioDetalle,
			[DOC_TCK] AS NumeroAtencion,
			ISNULL([VAR_FIR], 0) AS 'XML',
			[NUM_CPE] AS NombreArchivo
		FROM [Fact].[O.RBajasCabecera] AS FRA INNER JOIN
			 [Fact].[O.EstadoDocumento] AS FED ON FRA.DOC_EST = FED.Id_ED
		WHERE (@Fechadesde = '' AND @Fechahasta = '' OR FRA.[FEC_ENV] BETWEEN @Fechadesde AND @Fechahasta) --AND
			  AND FRA.[TIPO] = 'RA' AND FRA.RucEntity = @RucEntity
	END


GO
/****** Object:  StoredProcedure [Fact].[O.R.Usp_ListaCabeceraRC]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[O.R.Usp_ListaCabeceraRC] --'2015-12-07','2015-12-12',0,0
@Fechadesde VARCHAR(10),
@Fechahasta VARCHAR(10),
--@Correlativodesde INT,
--@Correlativohasta INT,
@IdEstado INT,
@TipoFecha INT,
@RucEntity VARCHAR(11)
AS
	IF(@TipoFecha = 1)
	BEGIN
		SELECT
			CONVERT(INT, ROW_NUMBER() OVER(ORDER BY FRC.[ID_RBC])) AS Nro,
			FRC.[ID_RBC] AS Correlativo,
	 		FRC.[FEC_INI] AS FechaInicio,
			--CONVERT(VARCHAR(10),FRC.[FEC_INI], 110) AS FechaInicio,
			FRC.[FEC_FIN] AS FechaFin,
			--CONVERT(VARCHAR(10),FRC.[FEC_FIN], 110) AS FechaFin,

			--FRC.[FEC_ENV] AS FechaEnvio,
			--CONVERT(VARCHAR(50),FRC.[FEC_ENV], 131) AS FechaEnvio,
			FRC.[FEC_CAD] AS FechaEnvio,

			--CONVERT(VARCHAR(50),FRC.[FEC_ENV]) AS FechaEnvio,

			--FRC.[NUM_CPE] AS Secuencia,
			--RIGHT(CONVERT(VARCHAR(30), FRC.[NUM_CPE]),4) AS Secuencia,
			CONVERT(VARCHAR(10), FRC.[NUM_SEC]) AS Secuencia,

			FED.[Id_ED] AS IdEstado,
			FED.[Desc] AS Estado,
			--1 AS Estado,
			--FRC.[DOC_MSG] AS MensajeEnvio,

			COALESCE(SUBSTRING(FRC.[DOC_MSG], 0, 30),'...')  AS MensajeEnvio,

			FRC.[DOC_MSG] AS MensajeEnvioDetalle,
			FRC.[DOC_TCK] AS NumeroAtencion,

			--FRC.[VAR_FIR] AS 'XML',
			ISNULL(FRC.[VAR_FIR], 0) AS 'XML',

			FRC.[NUM_CPE] AS NombreArchivo
		FROM [Fact].[O.RBoletasCabecera] AS FRC INNER JOIN
			 [Fact].[O.EstadoDocumento] AS FED ON FRC.DOC_EST = FED.Id_ED
		WHERE (@Fechadesde = '' AND @Fechahasta = '' OR FRC.[FEC_INI] BETWEEN @Fechadesde AND @Fechahasta) --AND
			  --(@Estado = 0 OR FRC.IdEstado = @IdEstado)
			  AND FRC.RucEntity = @RucEntity
	END
	ELSE
	BEGIN
		SELECT
			CONVERT(INT, ROW_NUMBER() OVER(ORDER BY FRC.[ID_RBC])) AS Nro,
			FRC.[ID_RBC] AS Correlativo,
	 		FRC.[FEC_INI] AS FechaInicio,
			--CONVERT(VARCHAR(10),FRC.[FEC_INI], 110) AS FechaInicio,
			FRC.[FEC_FIN] AS FechaFin,
			--CONVERT(VARCHAR(10),FRC.[FEC_FIN], 110) AS FechaFin,

			--FRC.[FEC_ENV] AS FechaEnvio,

			FRC.[FEC_CAD] AS FechaEnvio,

			--CONVERT(VARCHAR(10),FRC.[FEC_ENV], 110) AS FechaEnvio,
			--FRC.[NUM_CPE] AS Secuencia,
			--RIGHT(CONVERT(VARCHAR(30), FRC.[NUM_CPE]),4) AS Secuencia,
			CONVERT(VARCHAR(10), FRC.[NUM_SEC]) AS Secuencia,
			FED.[Id_ED] AS IdEstado,
			FED.[Desc] AS Estado,
			--1 AS Estado,
			--FRC.[DOC_MSG] AS MensajeEnvio,
			COALESCE(SUBSTRING(FRC.[DOC_MSG], 0, 30),'...')  AS MensajeEnvio,
			FRC.[DOC_MSG] AS MensajeEnvioDetalle,
			FRC.[DOC_TCK] AS NumeroAtencion,
			--FRC.[VAR_FIR] AS 'XML',
			ISNULL(FRC.[VAR_FIR], 0) AS 'XML',
			FRC.[NUM_CPE] AS NombreArchivo
		FROM [Fact].[O.RBoletasCabecera] AS FRC INNER JOIN
			 [Fact].[O.EstadoDocumento] AS FED ON FRC.DOC_EST = FED.Id_ED
		WHERE (@Fechadesde = '' AND @Fechahasta = '' OR FRC.[FEC_ENV] BETWEEN @Fechadesde AND @Fechahasta) --AND
			  --(@Estado = 0 OR FRC.IdEstado = @IdEstado)
			  AND FRC.RucEntity = @RucEntity
	END

	--(@Correlativodesde = 0 AND @Correlativohasta = 0 OR FRC.[ID_RBC] BETWEEN @Correlativodesde AND @Correlativohasta) --AND
	--(@Correlativodesde = 0 AND @Correlativohasta = 0 OR FRC.[ID_RBC] BETWEEN @Correlativodesde AND @Correlativohasta) --AND

GO
/****** Object:  StoredProcedure [Fact].[O.R.Usp_ListaCabeceraRR]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[O.R.Usp_ListaCabeceraRR] --'2015-12-07','2015-12-12',0,2
@Fechadesde VARCHAR(10),
@Fechahasta VARCHAR(10),
--@Correlativodesde INT,
--@Correlativohasta INT,
@IdEstado INT,
@TipoFecha INT,
@RucEntity VARCHAR(11)
AS
	IF(@TipoFecha = 1)
	BEGIN
		SELECT
			CONVERT(INT, ROW_NUMBER() OVER(ORDER BY FRA.[ID_RAC])) AS Nro,
			FRA.[ID_RAC] AS Correlativo,
			FRA.[FEC_ANU] AS FechaInicio,
			--CONVERT(VARCHAR(10),FRA.[FEC_ANU], 110) AS FechaInicio,
			--CONVERT(date, CONVERT(VARCHAR(10),FRA.[FEC_ANU], 110)) AS Fecha,
			FRA.[FEC_ANU] AS FechaFin,
			--CONVERT(VARCHAR(10),FRA.[FEC_ANU], 110) AS FechaFin,
			--FRA.[FEC_ENV] AS FechaEnvio,
			FRA.[FEC_CAD] AS FechaEnvio,
			--CONVERT(VARCHAR(10),FRA.[FEC_ENV], 110) AS FechaEnvio,
			CONVERT(VARCHAR,FRA.NUM_SEC) AS Secuencia,
			FED.[Id_ED] AS IdEstado,
			FED.[Desc] AS Estado,
			--1 AS Estado,
			--CONCAT(SUBSTRING([DOC_MSG], 0, 30),'...')  AS MensajeEnvio,
			SUBSTRING([DOC_MSG], 0, 30) + '...'  AS MensajeEnvio,
			[DOC_MSG] AS MensajeEnvioDetalle,
			[DOC_TCK] AS NumeroAtencion,
			
			--[VAR_FIR] AS 'XML',
			ISNULL([VAR_FIR], 0) AS 'XML',

			[NUM_CPE] AS NombreArchivo
		FROM [Fact].[O.RBajasCabecera] AS FRA INNER JOIN
			 [Fact].[O.EstadoDocumento] AS FED ON FRA.DOC_EST = FED.Id_ED
		WHERE (@Fechadesde = '' AND @Fechahasta = '' OR FRA.[FEC_ANU] BETWEEN @Fechadesde AND @Fechahasta) --AND
			  --(@Estado = 0 OR FRC.IdEstado = @IdEstado)
			  AND FRA.[TIPO] = 'RR' AND FRA.RucEntity = @RucEntity
	END
	ELSE
	BEGIN
		SELECT
			CONVERT(INT, ROW_NUMBER() OVER(ORDER BY FRA.[ID_RAC])) AS Nro,
			FRA.[ID_RAC] AS Correlativo,
			FRA.[FEC_ANU] AS FechaInicio,
			--CONVERT(VARCHAR(10),FRA.[FEC_ANU], 110) AS FechaInicio,
			--CONVERT(date, CONVERT(VARCHAR(10),FRA.[FEC_ANU], 110)) AS Fecha,
			FRA.[FEC_ANU] AS FechaFin,
			--CONVERT(VARCHAR(10),FRA.[FEC_ANU], 110) AS FechaFin,
			--FRA.[FEC_ENV] AS FechaEnvio,
			FRA.[FEC_CAD] AS FechaEnvio,
			--CONVERT(VARCHAR(10),FRA.[FEC_ENV], 110) AS FechaEnvio,
			CONVERT(VARCHAR,FRA.NUM_SEC) AS Secuencia,
			FED.[Id_ED] AS IdEstado,
			FED.[Desc] AS Estado,
			--1 AS Estado,
			--CONCAT (SUBSTRING([DOC_MSG], 0, 30),  '...') AS MensajeEnvio,
			SUBSTRING([DOC_MSG], 0, 30) + '...' AS MensajeEnvio,
			[DOC_MSG] AS MensajeEnvioDetalle,
			[DOC_TCK] AS NumeroAtencion,
			--[VAR_FIR] AS 'XML',
			ISNULL([VAR_FIR], 0) AS 'XML',
			[NUM_CPE] AS NombreArchivo
		FROM [Fact].[O.RBajasCabecera] AS FRA INNER JOIN
			 [Fact].[O.EstadoDocumento] AS FED ON FRA.DOC_EST = FED.Id_ED
		WHERE (@Fechadesde = '' AND @Fechahasta = '' OR FRA.[FEC_ENV] BETWEEN @Fechadesde AND @Fechahasta) --AND
			  --(@Estado = 0 OR FRC.IdEstado = @IdEstado)
			  AND FRA.[TIPO] = 'RR' AND FRA.RucEntity = @RucEntity
	END
		--(@Correlativodesde = 0 AND @Correlativohasta = 0 OR FRA.[ID_RAC] BETWEEN @Correlativodesde AND @Correlativohasta)

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ActualizaCE]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ActualizaCE]
	@NUM_CE [varchar](30),
   @ID_ED [varchar](2)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON
   UPDATE 
      [Fact].[O.DocumentoCabecera]
   SET
      [Id_ED] = @ID_ED
   WHERE
      NUM_CPE = @NUM_CE;

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ActualizaEstado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ActualizaEstado]
	@Id_DC [int],
   @Id_ED [varchar](2)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON
   UPDATE 
      [Fact].[O.DocumentoCabecera]
   SET
      [Id_ED] = @Id_ED,
	  [NRO_ENV] = ISNULL([NRO_ENV],0) + 1
   WHERE
      Id_DC = @Id_DC;

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ActualizaEstadoAnu]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ActualizaEstadoAnu]
	@Id_DC [int],
   @Id_ED [varchar](10)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON
   DECLARE @ID INT;
   SET @ID = (SELECT ID_ED FROM Fact.[O.EstadoDocumento] WHERE Abrev = @ID_ED)
   UPDATE 
      [Fact].[O.DocumentoCabecera]
   SET
      [Id_ED] = @ID
   WHERE
      VOIDED = @Id_DC;

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ActualizaEstadoRes]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ActualizaEstadoRes]
	@Id_DC [int],
   @Id_ED [varchar](10)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON
   DECLARE @ID INT;
   SET @ID = (SELECT ID_ED FROM Fact.[O.EstadoDocumento] WHERE Abrev = @ID_ED)
   UPDATE 
      [Fact].[O.DocumentoCabecera]
   SET
      [Id_ED] = @ID
   WHERE
      SUMMARY = @Id_DC;

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_GenerarXml]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_GenerarXml]
	@TipoDocumento [int],
	@Serie [varchar](4),
	@NumeroDocumento [varchar](15),
	@RucEmpresa [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN 
	SELECT 
		--[XML_ORIG]
		--,[XML_RES],
		FDC.[XML_SIGN]
	FROM [Fact].[O.DocumentoCabecera] AS FDC INNER JOIN
		[Ctl].[TipoDocumento] AS CTP ON FDC.ID_TPO_CPE = CTP.Code
	WHERE [SERIE] = @Serie AND
		FDC.[NUM_DOCUMENTO] = @NumeroDocumento AND
		CTP.Id_TD = @TipoDocumento AND
		FDC.EM_NUM_DOCU = @RucEmpresa
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_GetListaDetalleRA]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_GetListaDetalleRA]
	@Correlativo [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		 --FBC.[ID_RAC] AS IdFactura,
		 FBD.[ID_RAD] AS NroOrden, --Correlativo 
		 FBD.[TPO_CPE] AS TipoDocumento,
		 FBD.[DOC_SER] AS NumeroSerie,
		 CONVERT(VARCHAR, FBD.[DOC_NUM]) AS NumeroDocumento,
		 --'Fecha Anulacion del Doc.' AS FechaCarga,
		 [DOC_FEC] AS FechaCarga,

		 FBD.[DOC_DES] AS MotivoAnulacion,
		 FBD.[DOC_DES] AS MensajeDocumento
	FROM [Fact].[O.RBajasCabecera] AS FBC INNER JOIN
		 [Fact].[O.RBajasDetalle] AS FBD ON FBC.[ID_RAC] = FBD.[ID_RAC]
	WHERE FBC.[ID_RAC] = @Correlativo
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_GetListaDetalleRC]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_GetListaDetalleRC]
	@Correlativo [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		 FBD.[ID_RBD] AS Nro,
		 --FBC.[ID_RBC] AS IdFactura,
		 FBD.[TPO_CPE] AS TipoDocumento,
		 FBD.[DOC_SER] AS NumeroSerie,
		 FBD.[NUM_INI] AS NroInicio,
		 FBD.[NUM_FIN] AS NroFin,
		 (FBD.[NUM_FIN] - FBD.[NUM_INI] + 1) AS TotalDocEmitido,
		 CONVERT(VARCHAR, FBD.[MTO_EXO]) AS MontoExonerado,
		 CONVERT(VARCHAR, FBD.[MTO_GRA]) AS MontoGravado, 
		 CONVERT(VARCHAR, FBD.[MTO_INA]) AS MontoInafecto,
		 CONVERT(VARCHAR, FBD.[MTO_OCA]) AS TotalOtrosCargos,
		 CONVERT(VARCHAR, FBD.[IMP_ISC]) AS MontoISC,
		 CONVERT(VARCHAR, FBD.[IMP_IGV]) AS MontoIGV,
		 CONVERT(VARCHAR, FBD.[IMP_OTH]) AS TotalOtrosTributos,
		 CONVERT(VARCHAR, FBD.[MTO_TOT]) AS MontoTotal
	FROM [Fact].[O.RBoletasCabecera] AS FBC INNER JOIN
		 [Fact].[O.RBoletasDetalle] AS FBD ON FBC.ID_RBC = FBD.ID_RBC
	WHERE FBC.ID_RBC = @Correlativo
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_GetListaDetalleRR]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[O.Usp_GetListaDetalleRR] --1
@Correlativo INT
AS
BEGIN
	SELECT
		 --FBC.[ID_RAC] AS IdFactura,
		 FBD.[ID_RAD] AS NroOrden, --Correlativo 
		 FBD.[TPO_CPE] AS TipoDocumento,
		 FBD.[DOC_SER] AS NumeroSerie,
		 CONVERT(VARCHAR, FBD.[DOC_NUM]) AS NumeroDocumento,
		 --'Fecha Anulacion del Doc.' AS FechaCarga,
		 [DOC_FEC] AS FechaCarga,

		 FBD.[DOC_DES] AS MotivoAnulacion,
		 FBD.[DOC_DES] AS MensajeDocumento
	FROM [Fact].[O.RBajasCabecera] AS FBC INNER JOIN
		 [Fact].[O.RBajasDetalle] AS FBD ON FBC.[ID_RAC] = FBD.[ID_RAC]
	WHERE FBC.[ID_RAC] = @Correlativo
END
GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaAnulado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaAnulado]
	@NUM_CE [varchar](30),
	@VOIDED [varchar](30)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON
   UPDATE 
      [Fact].[O.DocumentoCabecera]
   SET
      [VOIDED] = @VOIDED
   WHERE
      NUM_CPE = @NUM_CE;

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaCabecera]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaCabecera]
	@NUM_CPE			   VARCHAR(50),
	@ID_TPO_CPE			CHAR(2),
	@ID_CPE				VARCHAR(25),
	@ID_TPO_OPERACION	CHAR(2),
	@FEC_EMIS			DATETIME,
	@TPO_MONEDA			CHAR(3),
	@TPO_NOTA			CHAR(2),
	@MOTIVO_NOTA		CHAR(250),
	@EM_TPO_DOC			CHAR(1),
	@EM_NUM_DOCU		CHAR(11),
	@EM_NOMBRE			VARCHAR(100),
	@EM_NCOMER			VARCHAR(100),
	@EM_UBIGEO			CHAR(6),
	@EM_DFISCAL			VARCHAR(100),
	@EM_DURBANIZ		VARCHAR(25),
	@EM_DIR_PROV		VARCHAR(30),
	@EM_DIR_DPTO		VARCHAR(30),
	@EM_DIR_DIST		VARCHAR(30),
	@EM_COD_PAIS		CHAR(2),
	@RE_TPODOC			CHAR(1),
	@RE_NUMDOC			CHAR(15),
	@RE_NOMBRE			CHAR(100),
	@RE_DIRECCION		VARCHAR(100),
	@TOT_GRAV_MTO		DECIMAL(12,2),
	@TOT_INAF_MTO		DECIMAL(12,2),
	@TOT_EXON_MTO		DECIMAL(12,2),
	@TOT_GRAT_MTO		DECIMAL(15,2),
	@TOT_DSCTO_MTO		DECIMAL(12,2),
	@TOT_SUMA_IGV		DECIMAL(12,2),
	@TOT_SUMA_ISC		DECIMAL(12,2),
	@TOT_SUMA_OTRIB		DECIMAL(12,2),
	@TOT_DCTO_GLOB		DECIMAL(12,2),
	@TOT_SUM_OCARG		DECIMAL(12,2),
	@ANT_TOT_ANTICIPO	DECIMAL(15,2),
	@TOT_IMPOR_TOTAL	DECIMAL(12,2),
	@MONTOLITERAL		VARCHAR(50),
	@PER_BASE_IMPO		DECIMAL(12,2),
	@PER_MNTO_PER		DECIMAL(12,2),
	@PER_MNTO_TOT		DECIMAL(12,2),
	@SERIE				VARCHAR(4),
	@NUM_DOCUMENTO		VARCHAR(15),
	@IdEstadoDocumento	INT,
   @VAR_FIR          VARBINARY(MAX),
	
	@RE_NCOMER VARCHAR (250),
	@RE_DURBANIZ VARCHAR (30),
	@RE_DIR_PROV VARCHAR (30),
	@RE_DIR_DPTO VARCHAR (30),
	@RE_DIR_DIST VARCHAR (30),
	@RE_COD_PAIS VARCHAR (2),
	@RE_UBIGEO VARCHAR (6),

	@REGIMENCE VARCHAR (2),
	@TASACE DECIMAL (12,2),
	@OBSCE VARCHAR (250),
	@IMPTOTCE DECIMAL (12,2),
	@MONIMPTOTCE VARCHAR (3),
	@IMPTOT DECIMAL (12,2),
	@MONIMPTOT VARCHAR (3),

	@SEDE VARCHAR(250),
	@USUARIO VARCHAR(200),
	@IMPRESORA VARCHAR(200),
	@CAMPO1 VARCHAR(255),
	@CAMPO2 VARCHAR(255),
	@CAMPO3 VARCHAR(255),
	@CAMPO4 VARCHAR(255),
	@CAMPO5 VARCHAR(255),
	@CAMPO6 VARCHAR(255),
	@CAMPO7 VARCHAR(255),
	@CAMPO8 VARCHAR(255),
	@CAMPO9 VARCHAR(255),
	@CAMPO10 VARCHAR(255)
AS

/*
----------------------------------------------------------------------------
-- Object Name: [Facturacion].[Usp_InsertaCabecera]
-- Project: Facturación Electrónica
-- Business Process: *
-- Purpose: Guardar la cabecera del documento
-- Detailed Description: Inserta el registro del documento en la base de datos
--                       y también actualiza la tabla de series
-- Database: BD_Facturacion
-- Dependent Objects: None
-- Called By: Documentos
-- Upstream Systems: N\A
-- Downstream Systems: N\A
-- 
--------------------------------------------------------------------------------------
-- Rev | CMR | Date Modified | Developer  | Change Summary
--------------------------------------------------------------------------------------
-- 001 | N\A | 22.10.2015    | JPespinoza | Codigo Original
--
*/

SET NOCOUNT ON
-- Ahora ingreso la cabecera
--SET IDENTITY_INSERT [Fact].[O.DocumentoCabecera] ON;
INSERT INTO [Fact].[O.DocumentoCabecera]
           --([IdDocumentoCabecera]
           ([NUM_CPE]
           ,[ID_TPO_CPE]
           ,[ID_CPE]
           ,[ID_TPO_OPERACION]
           ,[FEC_EMIS]
		   ,[TPO_MONEDA]
		   ,[TPO_NOTA]
		   ,[MOTIVO_NOTA]
		   ,[EM_TPO_DOC]
		   ,[EM_NUM_DOCU]
		   ,[EM_NOMBRE]
		   ,[EM_NCOMER]
		   ,[EM_UBIGEO]
		   ,[EM_DFISCAL]
		   ,[EM_DURBANIZ]
		   ,[EM_DIR_PROV]
		   ,[EM_DIR_DPTO]
		   ,[EM_DIR_DIST]
		   ,[EM_COD_PAIS]
		   ,[RE_TPODOC]
		   ,[RE_NUMDOC]
		   ,[RE_NOMBRE]
		   ,[RE_DIRECCION]
		   ,[TOT_GRAV_MTO]
		   ,[TOT_INAF_MTO]
		   ,[TOT_EXON_MTO]
		   ,[TOT_GRAT_MTO]
		   ,[TOT_DSCTO_MTO]
		   ,[TOT_SUMA_IGV]
		   ,[TOT_SUMA_ISC]
		   ,[TOT_SUMA_OTRIB]
		   ,[TOT_DCTO_GLOB]
		   ,[TOT_SUM_OCARG]
		   ,[ANT_TOT_ANTICIPO]
		   ,[TOT_IMPOR_TOTAL]
		   ,[MONTOLITERAL]
		   ,[PER_BASE_IMPO]
		   ,[PER_MNTO_PER]
		   ,[PER_MNTO_TOT]
		   ,[SERIE]
		   ,[NUM_DOCUMENTO]
		   ,[Id_ED]
         ,[VAR_FIR]

		   ,[RE_NCOMER], [RE_DURBANIZ], [RE_DIR_PROV], [RE_DIR_DPTO], [RE_DIR_DIST], [RE_COD_PAIS]
		   ,[RE_UBIGEO], [REGIMENCE], [TASACE], [OBSCE], [IMPTOTCE], [MONIMPTOTCE], [IMPTOT]
		   ,[MONIMPTOT], [SEDE], [USUARIO], [IMPRESORA], [CAMPO1], [CAMPO2]
		   ,[CAMPO3], [CAMPO4], [CAMPO5], [CAMPO6], [CAMPO7], [CAMPO8], [CAMPO9], [CAMPO10],[REF_FILES]
         ,[SYS_DATE],[NRO_ENV]
		   )
	 OUTPUT inserted.Id_DC  
     VALUES
           --@IdDocumentoCabecera
		   (@NUM_CPE
           ,@ID_TPO_CPE
           ,@ID_CPE
           ,@ID_TPO_OPERACION
           ,@FEC_EMIS
           ,@TPO_MONEDA
		   ,@TPO_NOTA
		   ,@MOTIVO_NOTA
		   ,@EM_TPO_DOC
		   ,@EM_NUM_DOCU
		   ,@EM_NOMBRE
		   ,@EM_NCOMER
		   ,@EM_UBIGEO
		   ,@EM_DFISCAL
		   ,@EM_DURBANIZ
		   ,@EM_DIR_PROV
		   ,@EM_DIR_DPTO
		   ,@EM_DIR_DIST
		   ,@EM_COD_PAIS
		   ,@RE_TPODOC
		   ,@RE_NUMDOC
		   ,@RE_NOMBRE
		   ,@RE_DIRECCION
		   ,@TOT_GRAV_MTO
		   ,@TOT_INAF_MTO
		   ,@TOT_EXON_MTO
		   ,@TOT_GRAT_MTO
		   ,@TOT_DSCTO_MTO
		   ,@TOT_SUMA_IGV
		   ,@TOT_SUMA_ISC
		   ,@TOT_SUMA_OTRIB
		   ,@TOT_DCTO_GLOB
		   ,@TOT_SUM_OCARG
		   ,@ANT_TOT_ANTICIPO
		   ,@TOT_IMPOR_TOTAL
		   ,@MONTOLITERAL
		   ,@PER_BASE_IMPO
		   ,@PER_MNTO_PER
		   ,@PER_MNTO_TOT
		   ,@SERIE
		   ,@NUM_DOCUMENTO
		   ,'2'
         ,@VAR_FIR

		   ,@RE_NCOMER, @RE_DURBANIZ, @RE_DIR_PROV, @RE_DIR_DPTO, @RE_DIR_DIST, @RE_COD_PAIS, @RE_UBIGEO
		   ,@REGIMENCE, @TASACE, @OBSCE, @IMPTOTCE, @MONIMPTOTCE, @IMPTOT, @MONIMPTOT
		   ,@SEDE, @USUARIO, @IMPRESORA, @CAMPO1, @CAMPO2, @CAMPO3, @CAMPO4, @CAMPO5, @CAMPO6, @CAMPO7
		   ,@CAMPO8, @CAMPO9, @CAMPO10, @CAMPO1
         ,CURRENT_TIMESTAMP
		 ,0
		   )
EXEC [Fact].[O.Usp_InsertaSerie] @SERIE,@ID_TPO_CPE,@EM_NUM_DOCU;
--SET IDENTITY_INSERT [Facturacion].[DocumentoCabecera] OFF;

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaCabeceraRA]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaCabeceraRA]
	@NUM_CPE	   VARCHAR(50),
	@TOT_DOC	   INT,
	@FEC_REF 	DATETIME,
	@FEC_ANU    DATETIME,
	@FEC_ENV 	DATETIME,
	@DOC_EST	   VARCHAR(10),
	@NUM_SEC    INT,
	@FEC_CAD    VARCHAR(25),
	@TIPO       VARCHAR(2),
   @RUC        VARCHAR(11),
   @VAR_FIR    VARBINARY(MAX)
AS

/*
----------------------------------------------------------------------------
-- Object Name: [Fact].[Usp_InsertaCabeceraRB]
-- Project: Facturación Electrónica
-- Business Process: *
-- Purpose: Guardar la cabecera del documento del Rsumen de Boletas
-- Detailed Description: Inserta el registro del documento en la base de datos
--                       y también actualiza la tabla de series
-- Database: BD_Facturacion
-- Dependent Objects: None
-- Called By: Documentos
-- Upstream Systems: N\A
-- Downstream Systems: N\A
-- 
--------------------------------------------------------------------------------------
-- Rev | CMR | Date Modified | Developer  | Change Summary
--------------------------------------------------------------------------------------
-- 001 | N\A | 04.02.2016    | JPespinoza | Codigo Original
--
*/

SET NOCOUNT ON
-- Ahora ingreso la cabecera
INSERT INTO [Fact].[O.RBajasCabecera]
   (
	[NUM_CPE],
	[TOT_DOC],
	[FEC_REF],
	[FEC_ANU],
	[FEC_ENV],
	[DOC_EST],
	[NUM_SEC],
	[FEC_CAD],
	[TIPO],
   [RucEntity],
   [VAR_FIR],
   [SYS_DATE]
   )
   OUTPUT inserted.ID_RAC  
   VALUES
   (
	@NUM_CPE,
	@TOT_DOC,
	@FEC_REF,
	@FEC_ANU,
	@FEC_ENV,
	'2',
	@NUM_SEC,
	@FEC_CAD,
	@TIPO,
   @RUC,
   @VAR_FIR,
   CURRENT_TIMESTAMP
   )

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaCabeceraRB]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaCabeceraRB]
	@NUM_CPE		VARCHAR(50),
	@TOT_DOC		INT,
	@FEC_INI 	DATETIME,
	@FEC_FIN    DATETIME,
   @FEC_ENV    DATETIME,
	@MTO_GRA		DECIMAL(15, 2),
	@MTO_EXO		DECIMAL(15, 2),
	@MTO_INA    DECIMAL(15, 2),
	@MTO_OCA 	DECIMAL(15, 2),
	@IMP_IGV		DECIMAL(15, 2),
	@IMP_ISC		DECIMAL(15, 2),
	@IMP_OTH		DECIMAL(15, 2),
	@MTO_TOT		DECIMAL(15, 2),
	@DOC_EST		VARCHAR(10),
   @NUM_SEC    INT,
   @FEC_CAD    VARCHAR(25),
   @RUC        VARCHAR(11),
   @VAR_FIR    VARBINARY(MAX)
AS

/*
----------------------------------------------------------------------------
-- Object Name: [Fact].[Usp_InsertaCabeceraRB]
-- Project: Facturación Electrónica
-- Business Process: *
-- Purpose: Guardar la cabecera del documento del Rsumen de Boletas
-- Detailed Description: Inserta el registro del documento en la base de datos
--                       y también actualiza la tabla de series
-- Database: BD_Facturacion
-- Dependent Objects: None
-- Called By: Documentos
-- Upstream Systems: N\A
-- Downstream Systems: N\A
-- 
--------------------------------------------------------------------------------------
-- Rev | CMR | Date Modified | Developer  | Change Summary
--------------------------------------------------------------------------------------
-- 001 | N\A | 04.02.2016    | JPespinoza | Codigo Original
--
*/

SET NOCOUNT ON
-- Ahora ingreso la cabecera
INSERT INTO [Fact].[O.RBoletasCabecera]
   (
   [NUM_CPE],
   [TOT_DOC],
   [FEC_INI],
   [FEC_FIN],
   [FEC_ENV],
   [MTO_GRA],
   [MTO_EXO],
   [MTO_INA],
   [MTO_OCA],
   [IMP_IGV],
   [IMP_ISC],
   [IMP_OTH],
   [MTO_TOT],
   [DOC_EST],
   [NUM_SEC],
   [FEC_CAD],
   [RucEntity],
   [VAR_FIR],
   [SYS_DATE]
   )
   OUTPUT inserted.ID_RBC  
   VALUES
   (
   @NUM_CPE,
   @TOT_DOC,
	@FEC_INI,
	@FEC_FIN,
	@FEC_ENV,
	@MTO_GRA,
	@MTO_EXO,
	@MTO_INA,
	@MTO_OCA,
	@IMP_IGV,
	@IMP_ISC,
	@IMP_OTH,
	@MTO_TOT,
	'2',
   @NUM_SEC,
   @FEC_CAD,
   @RUC,
   @VAR_FIR,
   CURRENT_TIMESTAMP
   )


GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaCDR]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaCDR]
	@NUM_CE [varchar](30),
	@VAR_RES [varbinary](max),
   @ID_ED [varchar](2)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON
   UPDATE 
      [Fact].[O.DocumentoCabecera]
   SET
      [VAR_RES] = @VAR_RES,
      [Id_ED] = @ID_ED,
      [CDR_DATE] = CURRENT_TIMESTAMP
   WHERE
      NUM_CPE = @NUM_CE;

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaDetalle]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Fact].[O.Usp_InsertaDetalle]
	@IdDocumentoCabecera int, 
	@NUM_CPE		   VARCHAR (50),
	@IT_NRO_ORD		CHAR (3),
	@IT_UND_MED		CHAR (3),
	@IT_CANT_ITEM	numeric(18,5),
	@IT_COD_PROD	CHAR (30),
	@IT_DESCRIP		VARCHAR (250),
	@IT_VAL_UNIT	DECIMAL (12, 5),
	@IT_MNT_PVTA	DECIMAL (12, 5),
	@IT_VAL_VTA		DECIMAL (12, 5),
	@IT_MTO_IGV		DECIMAL (12, 5),
	@IT_COD_AFE_IGV varchar(4) ,
	@IT_MTO_ISC		DECIMAL (12, 5),
	@IT_SIS_AFE_ISC DECIMAL (12, 5),
	@IT_DESC_MNTO	DECIMAL (12, 5),
	@SERIE				VARCHAR(4),
	@NUM_DOCUMENTO		VARCHAR(15),

	@TPODOCRELAC VARCHAR(2),
	@NUMDOCRELAC VARCHAR(13),
	@FEMISDOCRELAC DATETIME,
	@ITOTDOCRELAC DECIMAL (12,2),
	@MDOCRELAC VARCHAR(3),
	@FECMOVI DATETIME,
	@NUMMOVI VARCHAR(9),
	@IMPSOPERMOV DECIMAL (12,2),
	@MONMOVI VARCHAR(3),
	@IMPOPER DECIMAL (12,2),
	@MONIMPOPER VARCHAR(3),
	@FECOPER DATETIME,
	@IMPTOTOPER DECIMAL (12,2),
	@MONOPER VARCHAR(3),
	@MONREFETC VARCHAR(3),
	@MONDESTTC VARCHAR(3),
	@FACTORTC VARCHAR(11),
	@FECHATC DATETIME
AS

/*
----------------------------------------------------------------------------
-- Object Name: dbo.sp_InsertDetail
-- Project: Facturación Electrónica
-- Business Process: *
-- Purpose: Guardar la linea de detalle del documento
-- Detailed Description: Inserta el registro del documento en la base de datos
-- Database: Sistema_Facturación
-- Dependent Objects: None
-- Called By: Documentos
-- Upstream Systems: N\A
-- Downstream Systems: N\A
-- 
--------------------------------------------------------------------------------------
-- Rev | CMR | Date Modified | Developer  | Change Summary
--------------------------------------------------------------------------------------
-- 001 | N\A | 26.10.2015    | JPespinoza | Codigo Original
--
*/

SET NOCOUNT ON

-- 1 - Declare variables

-- 2 - Initialize variables

-- 3 - Execute INSERT command
--SET IDENTITY_INSERT [Facturacion].[DocumentoDetalle] ON;
INSERT INTO [Fact].[O.DocumentoDetalle]
   ([Id_DC]
   ,[NUM_CPE]
   ,[IT_NRO_ORD]
   ,[IT_UND_MED]
   ,[IT_CANT_ITEM]
   ,[IT_COD_PROD]
   ,[IT_DESCRIP]
   ,[IT_VAL_UNIT]
   ,[IT_MNT_PVTA]
   ,[IT_VAL_VTA]
   ,[IT_MTO_IGV]
   ,[IT_COD_AFE_IGV]
   ,[IT_MTO_ISC]
   ,[IT_SIS_AFE_ISC]
   ,[IT_DESC_MNTO]
   ,[SERIE]
   ,[NUM_DOCUMENTO]

   ,[TPODOCRELAC], [NUMDOCRELAC], [FEMISDOCRELAC], [ITOTDOCRELAC], [MDOCRELAC], [FECMOVI]
   ,[NUMMOVI], [IMPSOPERMOV], [MONMOVI], [IMPOPER], [MONIMPOPER], [FECOPER]
   ,[IMPTOTOPER], [MONOPER], [MONREFETC], [MONDESTTC], [FACTORTC], [FECHATC]
   )
     VALUES
   (@IdDocumentoCabecera
   ,@NUM_CPE
   ,@IT_NRO_ORD
   ,@IT_UND_MED
   ,@IT_CANT_ITEM
   ,@IT_COD_PROD
   ,@IT_DESCRIP
   ,CONVERT(DECIMAL(12,5), @IT_VAL_UNIT)
   ,CONVERT(DECIMAL(12,5), @IT_MNT_PVTA)
   ,CONVERT(DECIMAL(12,5), @IT_VAL_VTA)
   ,CONVERT(DECIMAL(12,5), @IT_MTO_IGV)
   ,@IT_COD_AFE_IGV
   ,CONVERT(DECIMAL(12,5), @IT_MTO_ISC)
   ,CONVERT(DECIMAL(12,5), @IT_SIS_AFE_ISC)
   ,CONVERT(DECIMAL(12,5), @IT_DESC_MNTO)
   ,@SERIE
   ,@NUM_DOCUMENTO

   ,@TPODOCRELAC, @NUMDOCRELAC, @FEMISDOCRELAC, @ITOTDOCRELAC, @MDOCRELAC, @FECMOVI
   ,@NUMMOVI, @IMPSOPERMOV, @MONMOVI, @IMPOPER, @MONIMPOPER, @FECOPER
   ,@IMPTOTOPER, @MONOPER, @MONREFETC, @MONDESTTC, @FACTORTC, @FECHATC
   )
--SET IDENTITY_INSERT [Facturacion].[DocumentoDetalle] OFF;


GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaDetalleRA]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaDetalleRA]
	@ID_RAC [int],
	@NUM_CPE [char](50),
	@TPO_CPE [char](4),
	@DOC_SER [char](6),
	@DOC_NUM [int],
	@DOC_DES [varchar](250),
	@DOC_FEC [varchar](10)
WITH EXECUTE AS CALLER
AS
/*
----------------------------------------------------------------------------
-- Object Name: [Fact].[Usp_InsertaDetalleRA]
-- Project: Facturación Electrónica
-- Business Process: *
-- Purpose: Guardar el detalle del documento del Rsumen de Documentos Anulados
-- Detailed Description: Inserta el registro del documento en la base de datos
-- Database: BD_Facturacion
-- Dependent Objects: None
-- Called By: Documentos
-- Upstream Systems: N\A
-- Downstream Systems: N\A
-- 
--------------------------------------------------------------------------------------
-- Rev | CMR | Date Modified | Developer  | Change Summary
--------------------------------------------------------------------------------------
-- 001 | N\A | 04.02.2016    | JPespinoza | Codigo Original
--
*/

SET NOCOUNT ON
-- Ahora ingreso la cabecera
INSERT INTO [Fact].[O.RBajasDetalle]
   (
	[ID_RAC],
	[NUM_CPE],
	[TPO_CPE],
	[DOC_SER],
	[DOC_NUM],
	[DOC_DES],
	[DOC_FEC]
   )
   VALUES
   (
	@ID_RAC,
	@NUM_CPE,
	@TPO_CPE,
	@DOC_SER,
	CONVERT(VARCHAR,@DOC_NUM),
	@DOC_DES,
	@DOC_FEC
   )

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaDetalleRB]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaDetalleRB]
	@ID_RBC [int],
	@NUM_CPE [char](50),
	@TPO_CPE [char](4),
	@DOC_SER [char](6),
	@NUM_INI [int],
	@NUM_FIN [int],
	@MTO_GRA [decimal](15, 2),
	@MTO_EXO [decimal](15, 2),
	@MTO_INA [decimal](15, 2),
	@MTO_OCA [decimal](15, 2),
	@IMP_IGV [decimal](15, 2),
	@IMP_ISC [decimal](15, 2),
	@IMP_OTH [decimal](15, 2),
	@MTO_TOT [decimal](15, 2),
	@NRO_LIN [int]
WITH EXECUTE AS CALLER
AS
/*
----------------------------------------------------------------------------
-- Object Name: [Fact].[Usp_InsertaDetalleRB]
-- Project: Facturación Electrónica
-- Business Process: *
-- Purpose: Guardar el detalle del documento del Rsumen de Boletas
-- Detailed Description: Inserta el registro del documento en la base de datos
-- Database: BD_Facturacion
-- Dependent Objects: None
-- Called By: Documentos
-- Upstream Systems: N\A
-- Downstream Systems: N\A
-- 
--------------------------------------------------------------------------------------
-- Rev | CMR | Date Modified | Developer  | Change Summary
--------------------------------------------------------------------------------------
-- 001 | N\A | 04.02.2016    | JPespinoza | Codigo Original
--
*/

SET NOCOUNT ON
-- Ahora ingreso la cabecera
INSERT INTO [Fact].[O.RBoletasDetalle]
   (
   [ID_RBC],
   [NUM_CPE],
   [TPO_CPE],
   [DOC_SER],
   [NUM_INI],
   [NUM_FIN],
   [MTO_GRA],
   [MTO_EXO],
   [MTO_INA],
   [MTO_OCA],
   [IMP_IGV],
   [IMP_ISC],
   [IMP_OTH],
   [MTO_TOT],
   [NRO_LIN]
   )
   VALUES
   (
   @ID_RBC,
	@NUM_CPE,
	@TPO_CPE,
   @DOC_SER,
	@NUM_INI,
	@NUM_FIN,
	@MTO_GRA,
	@MTO_EXO,
	@MTO_INA,
	@MTO_OCA,
	@IMP_IGV,
	@IMP_ISC,
	@IMP_OTH,
	@MTO_TOT,
   @NRO_LIN
   )

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaDocAfectad]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaDocAfectad]
	@ID_DC [int],
	@NRO_ORD [varchar](3),
	@DOC_AFEC_ID [varchar](15),
	@ID_TPO_CPE [varchar](2)
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Fact].[O.Afectado]
		(ID_DC, NRO_ORD, DOC_AFEC_ID, ID_TPO_CPE)
		VALUES
		(@ID_DC, @NRO_ORD, @DOC_AFEC_ID, @ID_TPO_CPE)
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaDocAnticip]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaDocAnticip]
	@ID_DC [int],
	@ANT_NROORDEN [varchar](3),
	@ANT_MONTO [decimal](12, 2),
	@ANT_TPO_DOC_ANT [varchar](2),
	@ANT_ID_DOC_ANT [varchar](20),
	@ANT_NUM_DOC_EMI [varchar](3),
	@ANT_TPO_DOC_EMI [varchar](2)
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Fact].[O.Anticipo]
		(ID_DC, ANT_NROORDEN, ANT_MONTO, ANT_TPO_DOC_ANT, ANT_ID_DOC_ANT, 
		ANT_NUM_DOC_EMI, ANT_TPO_DOC_EMI)
		VALUES
		(@ID_DC, @ANT_NROORDEN, @ANT_MONTO, @ANT_TPO_DOC_ANT,
		@ANT_ID_DOC_ANT, @ANT_NUM_DOC_EMI, @ANT_TPO_DOC_EMI)
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaDocDetra]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[O.Usp_InsertaDocDetra]
@ID_DC INT,
@NUMCPE VARCHAR(50),
@VAL_BBSS VARCHAR(50),
@CTA_BN   VARCHAR(50),
@PORCENT  VARCHAR(50),
@MONTO    VARCHAR(50)
AS
BEGIN
	INSERT INTO [Fact].[O.Detracciones]
		(ID_DC, NUM_CPE, VAL_BBSS, CTA_BN, PORCENT, MONTO)
		VALUES
		(@ID_DC, @NUMCPE, @VAL_BBSS, @CTA_BN, @PORCENT, @MONTO)
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaDocExtr]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaDocExtr]
	@ID_DC [int],
	@EXLINEA [varchar](50),
	@EXDATO [varchar](50),
	@EXTIPO [varchar](50)
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Fact].[O.Extra]
		(ID_DC, EXLINEA, EXDATO, EXTIPO)
		VALUES
		(@ID_DC, @EXLINEA, @EXDATO, @EXTIPO)
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaDocRefer]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaDocRefer]
	@ID_DC [int],
	@REF_NROORDEN [varchar](3),
	@REF_ID [varchar](15),
	@REF_TPO_DOC [varchar](2)
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Fact].[O.Referencia]
		(ID_DC, REF_NROORDEN, REF_ID, 
		REF_TPO_DOC)
		VALUES
		(@ID_DC, @REF_NROORDEN, @REF_ID, 
		@REF_TPO_DOC)
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaDocumentoAnulado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaDocumentoAnulado]
	@IdTipoDocumento [int],
	@Serie [varchar](4),
	@NumeroDocumento [varchar](20),
	@FechaAnulado [datetime],
	@MotivoAnulado [varchar](100),
	@EstadoAnulado [int],
	@RucEmpresa [varchar](11),
	@CodigoTipoDocumento [varchar](2),
	@User [varchar](100)
WITH EXECUTE AS CALLER
AS
BEGIN 
DECLARE @ID_ST INT = (SELECT Id_ED FROM [Fact].[O.EstadoDocumento] WHERE Abrev = 'ANS')
--SELECT @ID_ST
		INSERT INTO [Fact].[O.Documento_Anul]
			([Id_TpoDoc]
			,[Serie]
			,[Correlativo]
			,[Fecha_Anul]
			,[Motivo_Anul]
			,[Estado_Anul]
			,[Mensaje_Anul]
			,[RucEmpresa]
			,[TypeDoc]
			,[NUM_CE], [Usuario], Id_ED)
			VALUES
			(@IdTipoDocumento
			,@Serie
			,@NumeroDocumento
			,@FechaAnulado
			,@MotivoAnulado
			,@EstadoAnulado
			,'Registrado Correctamente'
			,@RucEmpresa
			,@CodigoTipoDocumento,
			@RucEmpresa + '-' + @CodigoTipoDocumento + '-' + @Serie + '-' + @NumeroDocumento, @User, @ID_ST)	
	END
	BEGIN
		UPDATE [Fact].[O.DocumentoCabecera] SET
			  [Id_ED_DOC] = 1, Id_ED = @ID_ST
		WHERE --ID_TPO_CPE = @CodigoTipoDocumento AND
			  --SERIE = @Serie AND
			  --NUM_DOCUMENTO = @NumeroDocumento AND
			  --EM_NUM_DOCU = @RucEmpresa AND
			  NUM_CPE = @RucEmpresa + '-' + @CodigoTipoDocumento + '-' + @Serie + '-' + @NumeroDocumento
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaFlexMailCab]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaFlexMailCab]
	@ID_DC [int],
	@NUM_CPE [varchar](30),
	@PARA [varchar](max),
	@CC [varchar](max),
	@CCO [varchar](max)
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Fact].[O.FlexMailEnvio]
	(ID_DC, NUM_CPE, PARA, CC, CCO)
	VALUES
	(@ID_DC, @NUM_CPE, @PARA, @CC, @CCO)
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaNotasRespuesta]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaNotasRespuesta]
	@NUM_CPE [varchar](50),
	@ID_CPE [int],
	@ERR_COD [varchar](6),
	@ERR_TXT [varchar](200),
	@TIPO [char](1),
	@DOC_EST [varchar](5)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON
	INSERT INTO [Fact].[O.NotasRespuesta]
		([NUM_CPE]
		,[ID_CPE]
		,[ERR_COD]
		,[ERR_TXT]
		,[TIPO])
	VALUES
		(@NUM_CPE,
		@ID_CPE,
		@ERR_COD,
		@ERR_TXT,
		@TIPO);

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaRespuesta]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaRespuesta]
	@Id_DC [int],
	@DOC_EST [varchar](10),
	@DOC_MSG [varchar](200),
	@DOC_TCK [varchar](20),
	@TIPO [varchar](2)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON;
   IF @TIPO = 'A'
      BEGIN
         UPDATE 
            [Fact].[O.RBajasCabecera]
         SET 
            [DOC_EST] = @DOC_EST,
            [DOC_MSG] = @DOC_MSG,
            [DOC_TCK] = @DOC_TCK
         WHERE
            [ID_RAC] = @Id_DC;
        IF @DOC_EST = '7'
            EXEC Fact.[O.Usp_ActualizaEstadoAnu] @Id_DC, 'AAS'
      END
   ELSE IF @TIPO = 'B'
      BEGIN
         UPDATE 
            [Fact].[O.RBoletasCabecera]
         SET 
            [DOC_EST] = @DOC_EST,
            [DOC_MSG] = @DOC_MSG,
            [DOC_TCK] = @DOC_TCK
         WHERE
            [ID_RBC] = @Id_DC;
        IF @DOC_EST = '7'
          EXEC Fact.[O.Usp_ActualizaEstadoRes] @Id_DC, 'SOK'
      END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaRespuestaSunat]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaRespuestaSunat]
	@Id_DC [int],
	@DOC_COD [varchar](5),
	@DOC_MSG [varchar](200)
WITH EXECUTE AS CALLER
AS
BEGIN
      UPDATE 
         [Fact].[O.DocumentoCabecera]
      SET 
         [DOC_COD] = @DOC_COD,
         [DOC_MSG] = @DOC_MSG
      WHERE
         [Id_DC] = @Id_DC;
   END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaResumen]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaResumen]
	@NUM_CE [varchar](30),
	@SUMMARY [varchar](30)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON
   UPDATE 
      [Fact].[O.DocumentoCabecera]
   SET
      [SUMMARY] = @SUMMARY
   WHERE
      NUM_CPE = @NUM_CE;

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaRXMLResponse]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaRXMLResponse]
	@Id_DC [int],
	@VAR_RES [varbinary](max),
	@TIPO [varchar](2)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON;
   IF @TIPO = 'A'
      BEGIN
         UPDATE 
            [Fact].[O.RBajasCabecera]
         SET 
            [VAR_RES] = @VAR_RES
         WHERE
            [ID_RAC] = @Id_DC;
      END
   ELSE IF @TIPO = 'B'
      BEGIN
         UPDATE 
            [Fact].[O.RBoletasCabecera]
         SET 
            [VAR_RES] = @VAR_RES
         WHERE
            [ID_RBC] = @Id_DC;
      END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaRXMLSign]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaRXMLSign]
	@Id_DC [int],
	@VAR_FIR [varbinary](max),
	@TIPO [varchar](2)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON;
   IF @TIPO = 'A'
      BEGIN
         UPDATE 
            [Fact].[O.RBajasCabecera]
         SET 
            [VAR_FIR] = @VAR_FIR,
            [DOC_EST] = '2'
         WHERE
            [ID_RAC] = @Id_DC;
      END
   ELSE IF @TIPO = 'B'
      BEGIN
         UPDATE 
            [Fact].[O.RBoletasCabecera]
         SET 
            [VAR_FIR] = @VAR_FIR,
            [DOC_EST] = '2'
         WHERE
            [ID_RBC] = @Id_DC;
      END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaSerie]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaSerie]
	@SERIE				VARCHAR(4),
   @ID_TPO_CPE       CHAR(2),
   @EM_NUM_DOCU		CHAR(11)
AS
SET NOCOUNT ON
   DECLARE @IDEMP INT =( SELECT IdEmpresa FROM Mtro.Empresa WHERE Ruc = @EM_NUM_DOCU);
      DECLARE @IDTDO INT =( SELECT Id_TD FROM Ctl.TipoDocumento WHERE Code = @ID_TPO_CPE);
-- Primero inserto o actualizo la tabla series
IF NOT EXISTS ( SELECT Id_S FROM [Fact].[O.Serie] WHERE CodeDoc = @ID_TPO_CPE AND NumSerie = @SERIE AND IdEmpresa = @IDEMP )
   
	BEGIN
      
		INSERT INTO [Fact].[O.Serie]
		(
         [Id_TD],
         [CodeDoc],
         [NumSerie],
         [IdEmpresa]
      )
		VALUES
		(
         @IDTDO,
         @ID_TPO_CPE,
         @SERIE,
         @IDEMP
      )
	END
--ELSE
--	BEGIN
--	  UPDATE Facturacion.Serie
--	  SET IdEstado = CAST(SUBSTRING(@ID_CPE,6,8) AS INT)
--	  WHERE Id_Tipodoc = CAST(@ID_TPO_CPE AS INT) AND Num_Serie= SUBSTRING(@ID_CPE,1,4);
--	END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaTC]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaTC]
	@RUC [varchar](11),
	@FECHA [varchar](10),
    @MONEDA [varchar](3),
	@CAMBIO [decimal](10, 5)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON

IF NOT EXISTS ( SELECT CAMBIO FROM [Fact].[ExchangeRate] WHERE RucNumber = @RUC AND Fecha = CONVERT(DATETIME,@FECHA,20) AND Moneda = @MONEDA )
	BEGIN
		INSERT INTO [Fact].[ExchangeRate]
		(
         [RucNumber],
		 [Fecha],
         [Moneda],
         [Cambio]
      )
		VALUES
		(
         @RUC,
         CONVERT(DATETIME,@FECHA,20),
         @MONEDA,
         @CAMBIO
      )
	END
GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaXmlResOther]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaXmlResOther]
	@ID_CE [varchar](30),
	@VAR_RES [varbinary](max)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON
	UPDATE 
		[Fact].[O.DocumentoCabecera]
		SET
			[VAR_RES] = @VAR_RES,
			[Id_ED] = '7'
		WHERE
		[NUM_CPE] = @ID_CE;

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaXmlResponse]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaXmlResponse]
	@Id_DC [int],
	@VAR_RES [varbinary](max)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON
   UPDATE 
      [Fact].[O.DocumentoCabecera]
   SET
      [VAR_RES] = @VAR_RES,
      [Id_ED] = '7'
   WHERE
      Id_DC = @Id_DC;

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_InsertaXmlSign]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_InsertaXmlSign]
	@Id_DC [int],
	@VAR_FIR [varbinary](max)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON
   UPDATE 
      [Fact].[O.DocumentoCabecera]
   SET
      [VAR_FIR] = @VAR_FIR,
      [Id_ED] = '2'
   WHERE
      Id_DC = @Id_DC;

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ListaDocumentoAnulado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ListaDocumentoAnulado]
	@FechaDesde [varchar](40),
	@FechaHasta [varchar](40),
	@Serie [varchar](4),
	@IdTipoDocumento [int],
	@RucEmpresa [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		CONVERT(INT, ROW_NUMBER() OVER(ORDER BY DA.[Id_DocAnul])) AS IdDocumentoAnulado
		--DA.[Id_DocAnul] AS IdDocumentoAnulado
		,DA.[Id_TpoDoc] AS IdTipoDocumento
		,TD.[Code] AS CodigoTipoDocumento
		,DA.[Serie] 
		,DA.[Correlativo] AS NumeroDocumento
		,DA.[Fecha_Anul] AS FechaAnulado
		,DA.[Motivo_Anul] AS MotivoAnulado

		,DA.[Estado_Anul] AS EstadoAnulado
		,DA.[Mensaje_Anul] AS MensajeAnulado
		,DA.[Estado_Envio] AS EstadoEnvio
		
		,DA.[MensajeEnvio]
		,DA.[NumAtencion] AS NumeroAtencion
		,DA.Id_ED AS IdEstadoDoc
		,ED.[Desc] AS EstadoDesc
	FROM [Fact].[O.Documento_Anul] AS DA INNER JOIN
		 [Ctl].[TipoDocumento] AS TD ON DA.[Id_TpoDoc] = TD.Id_TD LEFT JOIN
         [Fact].[O.EstadoDocumento] AS ED ON DA.Id_ED = ED.Id_ED
	WHERE (@FechaDesde = '' AND @FechaHasta = '' OR DA.[Fecha_Anul] BETWEEN @FechaDesde AND @FechaHasta) AND
		  (@IdTipoDocumento = 0 OR TD.Id_TD = @IdTipoDocumento) AND
		  (@Serie = '' OR DA.Serie = @Serie) AND
		  (DA.[RucEmpresa] = @RucEmpresa)
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ListaDocumentoCabecera]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[O.Usp_ListaDocumentoCabecera] --'','',0,'','0','0',0,'','',''
@FechaInicio VARCHAR(10),
@FechaFin VARCHAR(10),
--@FechaInicio DATETIME,
--@FechaFin DATETIME,
@TipoDocumento INT,
@Serie VARCHAR(4),
@NumeroDocumentoInicio VARCHAR(20),
@NumeroDocumentoFin VARCHAR(20),
@Estado INT,
@Ruc VARCHAR(20),
@RazonSocial VARCHAR(100),
@RucEmpresaEmisor VARCHAR(11),
@Id_ED_DOC INT,
@NameSede VARCHAR(200)
AS
	BEGIN
		SELECT 
			CONVERT(INT, ROW_NUMBER() OVER(ORDER BY DC.Id_DC)) AS Nro,
			CTP.Code AS 'CodigoDocumento',
			[CTP].[Desc] AS Descripcion,
			CONVERT(VARCHAR(4),SUBSTRING(DC.ID_CPE,1,4)) AS Serie,
			DC.NUM_CPE AS NumeroDocumento,
			DC.NUM_DOCUMENTO AS NumeroFactura,
			DC.FEC_EMIS AS FechaEmision,
			DC.RE_NUMDOC AS NumDocCliente,

			RTRIM(DC.RE_NOMBRE) AS Cliente,

			DC.RE_DIRECCION AS ClienteDireccion,
			DC.TOT_IMPOR_TOTAL AS MontoTotal,
			--[FED].[Desc] AS Estado,
			[FED].[RutaImagen] AS RutaImagen
			,CASE DC.TPO_MONEDA 
			WHEN 'PEN' THEN 'Soles'
			WHEN 'USD' THEN 'Dólares' 
			END AS Moneda,

			DC.EM_NUM_DOCU AS EmpresaRuc,
			DC.EM_NOMBRE AS RazonSocial,
			DC.EM_DFISCAL AS EmpresaDireccion,
			DC.EM_UBIGEO AS EmpresaUbigeo,
			MD.Descripcion AS EmpresaUbigeoDesc,
			CTP.Id_TD AS IdTipoDocumento,
			ISNULL(DC.VAR_FIR, 0) AS 'XML',

			DC.[CAMPO1] AS Campo1,
			DC.[DOC_COD] AS CodeMessage,
			DC.[DOC_MSG] AS DocMessage,
			NR.ERR_COD AS CodeResponse,
			NR.ERR_TXT AS NoteResponse,
			CONVERT(INT,DC.[CAMPO2]) AS TypeFormat,


			CASE FED.Abrev
			when 'EEN' then 
			'Enviando, Intento # ' + CONVERT(VARCHAR(2), ISNULL(DC.[NRO_ENV], 0))
			ELSE
			FED.[Desc]
			end
			AS Estado
	FROM [Fact].[O.DocumentoCabecera] AS DC LEFT JOIN
		 [Ctl].[TipoDocumento] AS CTP ON DC.ID_TPO_CPE = CTP.Code LEFT JOIN
		 [Fact].[O.EstadoDocumento] AS FED ON DC.Id_ED = FED.Id_ED LEFT JOIN 
		 [Mtro].[Distrito] AS MD ON DC.EM_UBIGEO = MD.CodigoUbigeo LEFT JOIN
		 [Fact].[O.NotasRespuesta] AS NR ON DC.NUM_CPE = NR.NUM_CPE
	WHERE
		(@FechaInicio = '' AND @FechaFin = '' OR DC.FEC_EMIS BETWEEN @FechaInicio AND @FechaFin) AND
		(@TipoDocumento = 0 OR CTP.Id_TD = @TipoDocumento) AND
		(@Serie = '' OR DC.SERIE = @Serie) AND
		((@NumeroDocumentoInicio = '' AND @NumeroDocumentoFin = '') OR DC.NUM_DOCUMENTO BETWEEN @NumeroDocumentoInicio AND @NumeroDocumentoFin) AND
		(@Estado = 0 OR DC.Id_ED = @Estado) AND
		(@Ruc = '' OR DC.RE_NUMDOC = @Ruc) AND
		(@RazonSocial = '' OR DC.RE_NOMBRE LIKE @RazonSocial + '%') AND
		(DC.EM_NUM_DOCU = @RucEmpresaEmisor) AND
		(CTP.Code = '01' OR CTP.Code = '03' OR CTP.Code = '07' OR CTP.Code = '08')
		AND (@NameSede = '' OR DC.SEDE = @NameSede)
END
GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ListaDocumentoCabeceraCRECPE]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[O.Usp_ListaDocumentoCabeceraCRECPE] --'','',0,'','0','0',0,'','',''
@FechaInicio VARCHAR(10),
@FechaFin VARCHAR(10),
@TipoDocumento INT,
@Serie VARCHAR(4),
@NumeroDocumentoInicio VARCHAR(20),
@NumeroDocumentoFin VARCHAR(20),
@Estado INT,
@Ruc VARCHAR(20),
@RazonSocial VARCHAR(100),
@RucEmpresaEmisor VARCHAR(11),
@Id_ED_DOC INT,
@NameSede VARCHAR(200)
AS
	BEGIN
		SELECT 
			CONVERT(INT, ROW_NUMBER() OVER(ORDER BY DC.Id_DC)) AS Nro,
			CTP.Code AS 'CodigoDocumento',
			[CTP].[Desc] AS Descripcion,
			CONVERT(VARCHAR(4),SUBSTRING(DC.ID_CPE,1,4)) AS Serie,
			DC.NUM_CPE AS NumeroDocumento,
			DC.NUM_DOCUMENTO AS NumeroFactura,
			DC.FEC_EMIS AS FechaEmision,
			DC.RE_NUMDOC AS NumDocCliente,

			RTRIM(DC.RE_NOMBRE) AS Cliente,
			DC.RE_DIRECCION AS ClienteDireccion,
			DC.[IMPTOT] AS MontoTotal,
			[FED].[Desc] AS Estado,
			[FED].[RutaImagen] AS RutaImagen
			,CASE DC.TPO_MONEDA 
			WHEN 'PEN' THEN 'Soles'
			WHEN 'USD' THEN 'Dólares' 
			END AS Moneda,
			DC.EM_NUM_DOCU AS EmpresaRuc,
			DC.EM_NOMBRE AS RazonSocial,
			DC.EM_DFISCAL AS EmpresaDireccion,
			DC.EM_UBIGEO AS EmpresaUbigeo,
			MD.Descripcion AS EmpresaUbigeoDesc,
			CTP.Id_TD AS IdTipoDocumento,
			--DC.VAR_FIR AS 'XML'
			ISNULL(DC.VAR_FIR, 0) AS 'XML',
			DC.[CAMPO1] AS Campo1,
			DC.[DOC_COD] AS CodeMessage,
			DC.[DOC_MSG] AS DocMessage,
			NR.ERR_COD AS CodeResponse,
			NR.ERR_TXT AS NoteResponse,
			CONVERT(INT,DC.[CAMPO2]) AS TypeFormat
		FROM [Fact].[O.DocumentoCabecera] AS DC LEFT JOIN
			 [Ctl].[TipoDocumento] AS CTP ON DC.ID_TPO_CPE = CTP.Code LEFT JOIN
			 [Fact].[O.EstadoDocumento] AS FED ON DC.Id_ED = FED.Id_ED LEFT JOIN 
			 [Mtro].[Distrito] AS MD ON DC.EM_UBIGEO = MD.CodigoUbigeo LEFT JOIN
			 [Fact].[O.NotasRespuesta] AS NR ON DC.NUM_CPE = NR.NUM_CPE
		WHERE
			(@FechaInicio = '' AND @FechaFin = '' OR DC.FEC_EMIS BETWEEN @FechaInicio AND @FechaFin) AND
			(@TipoDocumento = 0 OR CTP.Id_TD = @TipoDocumento) AND
			(@Serie = '' OR DC.SERIE = @Serie) AND
			((@NumeroDocumentoInicio = '' AND @NumeroDocumentoFin = '') OR DC.NUM_DOCUMENTO BETWEEN @NumeroDocumentoInicio AND @NumeroDocumentoFin) AND
			--((CONVERT(INT,@NumeroDocumentoInicio) = 0 AND CONVERT(INT, @NumeroDocumentoFin) = 0) OR CONVERT(INT, DC.NUM_DOCUMENTO) BETWEEN CONVERT(INT, @NumeroDocumentoInicio) AND CONVERT(INT, @NumeroDocumentoFin)) AND
			(@Estado = 0 OR DC.Id_ED = @Estado) AND
			(@Ruc = '' OR DC.RE_NUMDOC = @Ruc) AND
			(@RazonSocial = '' OR DC.RE_NOMBRE LIKE @RazonSocial + '%') AND
			(DC.EM_NUM_DOCU = @RucEmpresaEmisor) AND
			(CTP.Code = '20' OR CTP.Code = '40')
			AND DC.SEDE = @NameSede

END
GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ListaDocumentoCabExcel]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ListaDocumentoCabExcel]
	@FechaInicio [varchar](10),
	@FechaFin [varchar](10),
	@TipoDocumento [int],
	@Serie [varchar](4),
	@NumeroDocumentoInicio [varchar](20),
	@NumeroDocumentoFin [varchar](20),
	@Estado [int],
	@Ruc [varchar](20),
	@RazonSocial [varchar](100),
	@RucEmpresa [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		CONVERT(INT, ROW_NUMBER() OVER(ORDER BY DC.Id_DC)) AS Nro,
		CTP.Code AS 'CodigoTipoDocumento',
		[CTP].[Desc] AS TipoDocumentoDesc,
		CONVERT(VARCHAR(4),SUBSTRING(DC.ID_CPE,1,4)) AS Serie,
		DC.NUM_CPE AS NombreDocumento,
		DC.NUM_DOCUMENTO AS NumeroDocumento,
		DC.FEC_EMIS AS FechaEmision,
		DC.RE_NUMDOC AS NumDocCliente,
		DC.RE_NOMBRE AS Cliente,

		--CASE DC.TPO_MONEDA 
		--WHEN 'PEN' THEN 'S/ ' + CONVERT(VARCHAR, DC.TOT_IMPOR_TOTAL)
		--WHEN 'USD' THEN '$ ' + CONVERT(VARCHAR, DC.TOT_IMPOR_TOTAL)
		--END AS Total,

		DC.TOT_IMPOR_TOTAL AS Total,

		--DC.TOT_IMPOR_TOTAL AS MontoTotal,
		[FED].[Desc] AS Estado,
		[FED].[RutaImagen] AS RutaImagen
		,CASE DC.TPO_MONEDA 
		WHEN 'PEN' THEN 'SOLES'
		WHEN 'USD' THEN 'DÓLARES AMERICANOS' 
		END AS Moneda
	FROM [Fact].[O.DocumentoCabecera] AS DC LEFT JOIN
		 [Ctl].[TipoDocumento] AS CTP ON DC.ID_TPO_CPE = CTP.Code LEFT JOIN
		 [Fact].[O.EstadoDocumento] AS FED ON DC.Id_ED = FED.Id_ED 
	WHERE
		(@FechaInicio = '' AND @FechaFin = '' OR DC.FEC_EMIS BETWEEN @FechaInicio AND @FechaFin) AND
		(@TipoDocumento = 0 OR CTP.Id_TD = @TipoDocumento) AND
		(@Serie = '' OR DC.SERIE = @Serie) AND
		((@NumeroDocumentoInicio = '' AND @NumeroDocumentoFin = '') OR DC.NUM_DOCUMENTO BETWEEN @NumeroDocumentoInicio AND @NumeroDocumentoFin) AND
		(@Estado = 0 OR DC.Id_ED = @Estado) AND
		(@Ruc = '' OR DC.RE_NUMDOC = @Ruc) AND
		(@RazonSocial = '' OR DC.RE_NOMBRE LIKE @RazonSocial + '%') AND
		(DC.EM_NUM_DOCU = @RucEmpresa)
END
GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ListaDocumentoDetalle]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ListaDocumentoDetalle]
	@TipoDocumento [int],
	@Serie [varchar](10),
	@NumeroDocumento [varchar](20),
	@RucEmpresa [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		CONVERT(INT, [IT_NRO_ORD]) AS NroOrden
		,[IT_UND_MED] AS Unidad
		,CONVERT(DECIMAL(18,2),[IT_CANT_ITEM]) AS Cantidad
		,[IT_COD_PROD] AS CodigoProducto
		,[IT_DESCRIP] AS Descripcion
		,CONVERT(DECIMAL(18,2),[IT_VAL_UNIT]) AS ValorVenta
		,CONVERT(DECIMAL(18,2),[IT_VAL_UNIT]) AS ValorUnitario
		,CONVERT(VARCHAR, CONVERT(DECIMAL(18,2),[IT_VAL_UNIT])) AS ValorVentaTexto

		,CONVERT(DECIMAL(18,2),[IT_MNT_PVTA]) AS PrecioVenta
		,CONVERT(VARCHAR, CONVERT(DECIMAL(18,2),[IT_MNT_PVTA])) AS PrecioVentaTexto

		--,CONVERT(money,[IT_MNT_PVTA]) AS precio
		,[IT_VAL_VTA]
		,[IT_MTO_IGV] AS Igv
		,CONVERT(DECIMAL(18,2),([IT_CANT_ITEM] * [IT_VAL_VTA])) AS SubTotal
		,CONVERT(VARCHAR, CONVERT(DECIMAL(18,2),([IT_CANT_ITEM] * [IT_VAL_VTA]))) AS SubTotalTexto

		,CONVERT(DECIMAL(18,2),([IT_CANT_ITEM] * [IT_MNT_PVTA])) AS Importe
		,[IT_MTO_IGV]
		,[IT_COD_AFE_IGV]
		,[IT_MTO_ISC]
		,[IT_SIS_AFE_ISC]
		,[IT_DESC_MNTO]
		,'' AS Simbolo
		,'' AS SimboloSol
		,FDD.[IT_DESC_MNTO] AS Dscto
	FROM [Fact].[O.DocumentoCabecera] AS FDC INNER JOIN
		 [Fact].[O.DocumentoDetalle] AS FDD ON FDC.[Id_DC] = FDD.[Id_DC] INNER JOIN
		 [Ctl].[TipoDocumento] AS CTD ON FDC.ID_TPO_CPE = CTD.Code
	WHERE FDD.[SERIE] = @Serie AND
		  FDD.[NUM_DOCUMENTO] = @NumeroDocumento AND
		  --FDC.ID_TPO_CPE = @TipoDocumento AND
		  CTD.Id_TD = @TipoDocumento AND
		  FDC.EM_NUM_DOCU = @RucEmpresa       
END
GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ListaEstadoDocumento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ListaEstadoDocumento]
WITH EXECUTE AS CALLER
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		0 AS IdEstado,
		'- Todos -' AS Descripcion
	UNION
	SELECT 
		[Id_ED] AS IdEstado,
		[Desc] AS Descripcion
	FROM [Fact].[O.EstadoDocumento]
END


GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ListaMontosCab]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ListaMontosCab]
	@TipoDocumento [int],
	@Serie [varchar](4),
	@NumeroDocumento [varchar](15),
	@RucEmpresa [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		CONVERT(VARCHAR(15), DC.TOT_SUMA_IGV) AS IGV
		,CONVERT(VARCHAR(15),DC.TOT_GRAV_MTO) AS TotalGravado
		,CONVERT(VARCHAR(15),DC.TOT_INAF_MTO) AS TotalnoGravado
		,CONVERT(VARCHAR(15),DC.TOT_EXON_MTO) AS TotalExonerado
		,CONVERT(VARCHAR(15),DC.TOT_DSCTO_MTO) AS TotalDescuento
		,CONVERT(VARCHAR(15),DC.TOT_IMPOR_TOTAL) AS MontoTotalCad
		,DC.MONTOLITERAL AS MontoTotalLetras
		,DC.RE_NOMBRE AS TipoMonto
		,CONVERT(VARCHAR(15),DC.TOT_IMPOR_TOTAL) AS TotalTipoMonto
		,DC.CodigoPDF417
		,DC.LogoEmpresa
		,DC.TOT_SUMA_IGV AS MontoTotal
	FROM [Fact].[O.DocumentoCabecera] AS DC INNER JOIN
		 [Ctl].[TipoDocumento] AS CTP ON DC.ID_TPO_CPE = CTP.Code
	WHERE DC.SERIE = @Serie AND
		  DC.NUM_DOCUMENTO = @NumeroDocumento AND
		  CTP.Id_TD = @TipoDocumento AND
		  DC.EM_NUM_DOCU = @RucEmpresa
END
GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ListaSerie]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ListaSerie] --0,0
	@TipoDocumento [int],
	@Empresa [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		FS.Id_S AS IdSerie
		,FS.NumSerie AS NumeroSerie
	FROM [Fact].[O.Serie] AS FS INNER JOIN 
		 [Ctl].[TipoDocumento] AS CTP ON FS.[CodeDoc] = CTP.[Code] 
	WHERE (@TipoDocumento = 0 OR CTP.[Id_TD] = @TipoDocumento) AND
		  (@Empresa = 0 OR FS.IdEmpresa = @Empresa)
		  --(@Empresa = 0 OR FS.IdEmpresa = @Empresa)
END
GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ObtenerDocumentoParaCorreo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ObtenerDocumentoParaCorreo]
@ruccomp VARCHAR(11)
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON

Declare @TempTableVariable 
   TABLE(
      CPE varchar(50),
  	  FEC DATETIME,
  	  TOT DECIMAL(12,2),
      EST VARCHAR(MAX),
      RZN VARCHAR(MAX)
   )
DECLARE @NRO_ENV int;
--EXEC @NRO_ENV = Fact.fn_GetNroEnv
SET @NRO_ENV = (SELECT MaxNumAttempts FROM Conf.TimeService WHERE CodeService = 'ADE.Services.SunatDelivery')
BEGIN
   -- INSERTA DOCUMENTOS UNITARIOS
   INSERT INTO @TempTableVariable(CPE, FEC, TOT, EST, RZN)
      SELECT-- TOP 1
         DC.NUM_CPE,
    		 DC.FEC_EMIS,
    		 DC.TOT_IMPOR_TOTAL,
         ED.[Desc],
         ME.RazonSocial
      FROM [Fact].[O.DocumentoCabecera] AS DC INNER JOIN [Fact].[O.EstadoDocumento] AS ED ON DC.Id_ED = ED.Id_ED
           INNER JOIN [Mtro].[Empresa] AS ME ON DC.EM_NUM_DOCU = ME.Ruc
      WHERE 
         DATEDIFF(MINUTE,DC.SYS_DATE,CURRENT_TIMESTAMP) > 1 AND
         DC.VAR_RES IS NULL AND
         DC.SYS_DATE IS NOT NULL AND
         --DC.NRO_ENV > @NRO_ENV AND
         DC.ID_TPO_CPE != '03' AND
         DC.SERIE NOT LIKE 'B%' AND
         ED.Abrev NOT IN('SOK', 'SOB', 'SRE', 'AAS') AND
         DC.EM_NUM_DOCU = @ruccomp 
		  OR
    		 DC.SYS_DATE IS NOT NULL AND
         --DC.NRO_ENV > @NRO_ENV AND
         DC.ID_TPO_CPE != '03' AND
         DC.SERIE NOT LIKE 'B%' AND
    		 ED.Abrev NOT IN('SOK', 'SOB', 'SRE', 'AAS') AND
         DC.EM_NUM_DOCU = @ruccomp
    --ORDER BY CONVERT(VARCHAR(10), DC.FEC_EMIS) ASC
    --ORDER BY CONVERT(VARCHAR(10), DC.FEC_EMIS) ASC
   -- INSERTA RESUMENES DE BOLETAS SIN ENVIAR A SUNAT
   INSERT INTO @TempTableVariable(CPE, FEC, TOT)
      SELECT 
         RTRIM(DC.NUM_CPE),
		 FEC_INI,
		 0
      FROM Fact.[O.RBoletasCabecera] AS DC
      WHERE 
         DATEDIFF(MINUTE,DC.FEC_ENV,CURRENT_TIMESTAMP) > 5 AND
         DC.VAR_RES IS NULL AND
         DC.DOC_TCK IS NULL

   -- INSERTA RESUMENES DE BAJAS Y REVERSIONES SIN ENVIAR A SUNAT
   INSERT INTO @TempTableVariable(CPE, FEC, TOT)
      SELECT 
         RTRIM(DC.NUM_CPE),
		 FEC_REF,
		 0
      FROM Fact.[O.RBajasCabecera] AS DC
      WHERE 
         DATEDIFF(MINUTE,DC.FEC_ENV,CURRENT_TIMESTAMP) > 5 AND
         DC.VAR_RES IS NULL AND
         DC.DOC_TCK IS NULL

   -- RESUMENES DE BOLETAS CON TICKET DE SUNAT
   INSERT INTO @TempTableVariable(CPE, FEC, TOT)
      SELECT 
         (RTRIM(DC.NUM_CPE) + ' Q') AS QRY,
		 FEC_INI,
		 0
      FROM Fact.[O.RBoletasCabecera] AS DC
      WHERE 
         DATEDIFF(MINUTE,DC.FEC_ENV,CURRENT_TIMESTAMP) > 5 AND
         DC.VAR_RES IS NULL AND
         DC.DOC_TCK IS NOT NULL
   
   -- RESUMENES DE BAJAS Y REVERSIONES CON TICKET DE SUNAT
   INSERT INTO @TempTableVariable(CPE, FEC, TOT)
      SELECT 
         (RTRIM(DC.NUM_CPE) + ' Q') AS QRY,
		 FEC_REF,
		 0
      FROM Fact.[O.RBajasCabecera] AS DC
      WHERE 
         DATEDIFF(MINUTE,DC.FEC_ENV,CURRENT_TIMESTAMP) > 5 AND
         DC.VAR_RES IS NULL AND
         DC.DOC_TCK IS NOT NULL

   -- ENVIA TODOS LOS DOCUMENTOS SELECCIONADOS
   SELECT * FROM @TempTableVariable ORDER BY FEC;
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ObtenerDocumentoParaEnviar]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ObtenerDocumentoParaEnviar]
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON

Declare @TempTableVariable 
   TABLE(
      CPE varchar(50),
      MAT INT
   )
DECLARE @NRO_ENV int;
--EXEC @NRO_ENV = Fact.fn_GetNroEnv
SET @NRO_ENV = (SELECT MaxNumAttempts FROM Conf.TimeService WHERE CodeService = 'ADE.Services.SunatDelivery')
BEGIN
   -- INSERTA DOCUMENTOS UNITARIOS
   INSERT INTO @TempTableVariable(CPE)
      SELECT-- TOP 1
         DC.NUM_CPE
      FROM [Fact].[O.DocumentoCabecera] AS DC
      WHERE 
         DATEDIFF(MINUTE,DC.SYS_DATE,CURRENT_TIMESTAMP) > 1 AND
         DC.VAR_RES IS NULL AND
         DC.SYS_DATE IS NOT NULL AND
         DC.NRO_ENV <= @NRO_ENV AND
         DC.ID_TPO_CPE != '03' AND
         --DC.ID_TPO_CPE != '20' AND
         DC.SERIE NOT LIKE 'B%' AND
         DC.Id_ED < 5

   -- INSERTA RESUMENES DE BOLETAS SIN ENVIAR A SUNAT
   INSERT INTO @TempTableVariable(CPE)
      SELECT 
         RTRIM(DC.NUM_CPE)
      FROM Fact.[O.RBoletasCabecera] AS DC
      WHERE 
         DATEDIFF(MINUTE,DC.FEC_ENV,CURRENT_TIMESTAMP) > 5 AND
         DC.VAR_RES IS NULL AND
         DC.DOC_TCK IS NULL

   -- INSERTA RESUMENES DE BAJAS Y REVERSIONES SIN ENVIAR A SUNAT
   INSERT INTO @TempTableVariable(CPE)
      SELECT 
         RTRIM(DC.NUM_CPE)
      FROM Fact.[O.RBajasCabecera] AS DC
      WHERE 
         DATEDIFF(MINUTE,DC.FEC_ENV,CURRENT_TIMESTAMP) > 5 AND
         DC.VAR_RES IS NULL AND
         DC.DOC_TCK IS NULL

   -- RESUMENES DE BOLETAS CON TICKET DE SUNAT
   INSERT INTO @TempTableVariable(CPE)
      SELECT 
         (RTRIM(DC.NUM_CPE) + ' Q') AS QRY
      FROM Fact.[O.RBoletasCabecera] AS DC
      WHERE 
         DATEDIFF(MINUTE,DC.FEC_ENV,CURRENT_TIMESTAMP) > 5 AND
         DC.VAR_RES IS NULL AND
         DC.DOC_TCK IS NOT NULL
   
   -- RESUMENES DE BAJAS Y REVERSIONES CON TICKET DE SUNAT
   INSERT INTO @TempTableVariable(CPE)
      SELECT 
         (RTRIM(DC.NUM_CPE) + ' Q') AS QRY
      FROM Fact.[O.RBajasCabecera] AS DC
      WHERE 
         DATEDIFF(MINUTE,DC.FEC_ENV,CURRENT_TIMESTAMP) > 5 AND
         DC.VAR_RES IS NULL AND
         DC.DOC_TCK IS NOT NULL

   -- ENVIA TODOS LOS DOCUMENTOS SELECCIONADOS
   SELECT CPE, ISNULL(@NRO_ENV,0) AS MAT FROM @TempTableVariable;
END

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ObtenerDocumentoPortalWeb]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ObtenerDocumentoPortalWeb] --9,'RRRU','00000001','20431084172','15/07/2016',3924.00,''
	@TipoDocumento [int],
	@Serie [varchar](4),
	@NroDocumento [varchar](20),
	@RucEmpresa [varchar](11),
	@FechaEmision [varchar](30),
	@MontoTotal [decimal](12, 2),
	@NroDocumentoCliente [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		CONVERT(INT, ROW_NUMBER() OVER(ORDER BY DC.Id_DC)) AS Nro,--
		--DC.Id_DC AS Nro,
		CTP.Code AS 'CodigoDocumento',
		[CTP].[Desc] AS Descripcion,
		CONVERT(VARCHAR(4),SUBSTRING(DC.ID_CPE,1,4)) AS Serie,
		DC.NUM_CPE AS NumeroDocumento,
		DC.NUM_DOCUMENTO AS NumeroFactura,
		DC.FEC_EMIS AS FechaEmision,
		DC.RE_NUMDOC AS NumDocCliente,
		DC.RE_NOMBRE AS Cliente,
		DC.RE_DIRECCION AS ClienteDireccion,
		DC.TOT_IMPOR_TOTAL AS MontoTotal,
		[FED].[Desc] AS Estado,
		[FED].[RutaImagen] AS RutaImagen  
		,CASE DC.TPO_MONEDA 
		WHEN 'PEN' THEN 'Soles'
		WHEN 'USD' THEN 'Dólares' 
		END AS Moneda,
		DC.EM_NUM_DOCU AS EmpresaRuc,
		DC.EM_NOMBRE AS RazonSocial,
		DC.EM_DFISCAL AS EmpresaDireccion,
		--DC.EM_UBIGEO AS EmpresaUbigeo,
		--MD.Descripcion AS EmpresaUbigeoDesc,
		'' AS EmpresaUbigeo,
		'' AS EmpresaUbigeoDesc,
		CTP.Id_TD AS IdTipoDocumento,
		--DC.[XML_SIGN] AS 'XML'

		--DC.[VAR_FIR] AS 'XML'
		ISNULL(DC.[VAR_FIR], 0) AS 'XML',
		DC.[CAMPO1] AS Campo1,
		CONVERT(INT,DC.[CAMPO2]) AS TypeFormat


		--CONVERT(INT, ROW_NUMBER() OVER(ORDER BY DC.Id_DC)) AS Nro,
		--CTP.Code AS 'CodigoDocumento',
		--[CTP].[Desc] AS Descripcion,
		--CONVERT(VARCHAR(4),SUBSTRING(DC.ID_CPE,1,4)) AS Serie,
		--DC.NUM_CPE AS NumeroDocumento,
		--DC.NUM_DOCUMENTO AS NumeroFactura,
		--DC.FEC_EMIS AS FechaEmision,
		--DC.RE_NUMDOC AS NumDocCliente,
		--RTRIM(DC.RE_NOMBRE) AS Cliente,
		--DC.RE_DIRECCION AS ClienteDireccion,
		--DC.TOT_IMPOR_TOTAL AS MontoTotal,
		--[FED].[Desc] AS Estado,
		--[FED].[RutaImagen] AS RutaImagen
		--,CASE DC.TPO_MONEDA 
		--WHEN 'PEN' THEN 'Soles'
		--WHEN 'USD' THEN 'Dólares' 
		--END AS Moneda,
		--DC.EM_NUM_DOCU AS EmpresaRuc,
		--DC.EM_NOMBRE AS RazonSocial,
		--DC.EM_DFISCAL AS EmpresaDireccion,
		--DC.EM_UBIGEO AS EmpresaUbigeo,
		--MD.Descripcion AS EmpresaUbigeoDesc,
		--CTP.Id_TD AS IdTipoDocumento,
		----DC.VAR_FIR AS 'XML'
		--ISNULL(DC.VAR_FIR, 0) AS 'XML',
		--DC.[CAMPO1] AS Campo1
	FROM [Fact].[O.DocumentoCabecera] AS DC LEFT JOIN
		 [Ctl].[TipoDocumento] AS CTP ON DC.[ID_TPO_CPE] = CTP.Code LEFT JOIN
		 [Fact].[O.EstadoDocumento] AS FED ON DC.Id_ED = FED.Id_ED --LEFT JOIN 
		 --[Mtro].[Distrito] AS MD ON DC.EM_UBIGEO = MD.CodigoUbigeo
	WHERE
		(DC.NUM_DOCUMENTO = @NroDocumento) AND
		(CTP.Id_TD = @TipoDocumento) AND
		(DC.SERIE = @Serie) AND
		(DC.EM_NUM_DOCU = @RucEmpresa) AND
		(DC.FEC_EMIS = CONVERT(DATETIME, @FechaEmision)) AND
		(DC.TOT_IMPOR_TOTAL = @MontoTotal) AND
		--(DC.RE_NUMDOC = @NroDocumentoCliente)
		(@NroDocumentoCliente = '' OR DC.RE_NUMDOC = @NroDocumentoCliente)
END












GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ObtenerDocumentoUnico]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ObtenerDocumentoUnico]
	@TipoDocumento [int],
	@Serie [varchar](4),
	@NroDocumento [varchar](20),
	@RucEmpresa [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		CONVERT(INT, ROW_NUMBER() OVER(ORDER BY DC.Id_DC)) AS Nro,
		CTP.Code AS 'CodigoDocumento',
		[CTP].[Desc] AS Descripcion,
		CONVERT(VARCHAR(4),SUBSTRING(DC.ID_CPE,1,4)) AS Serie,
		DC.NUM_CPE AS NumeroDocumento,
		DC.NUM_DOCUMENTO AS NumeroFactura,
		DC.FEC_EMIS AS FechaEmision,
		DC.RE_NUMDOC AS NumDocCliente,
		DC.RE_NOMBRE AS Cliente,
		DC.RE_DIRECCION AS ClienteDireccion,
		DC.TOT_IMPOR_TOTAL AS MontoTotal,
		[FED].[Desc] AS Estado,
		[FED].[RutaImagen] AS RutaImagen  
		,CASE DC.TPO_MONEDA 
		WHEN 'PEN' THEN 'Soles'
		WHEN 'USD' THEN 'Dólares' 
		END AS Moneda,
		DC.EM_NUM_DOCU AS EmpresaRuc,
		DC.EM_NOMBRE AS RazonSocial,
		DC.EM_DFISCAL AS EmpresaDireccion,
		DC.EM_UBIGEO AS EmpresaUbigeo,
		MD.Descripcion AS EmpresaUbigeoDesc,
		CTP.Id_TD AS IdTipoDocumento,
		--DC.[VAR_FIR] AS 'XML',
		ISNULL(DC.[VAR_FIR], 0) AS 'XML',
		DC.[CAMPO1] AS Campo1,
		CONVERT(INT,DC.[CAMPO2]) AS TypeFormat
	FROM [Fact].[O.DocumentoCabecera] AS DC LEFT JOIN
		 [Ctl].[TipoDocumento] AS CTP ON DC.ID_TPO_CPE = CTP.Code LEFT JOIN
		 [Fact].[O.EstadoDocumento] AS FED ON DC.Id_ED = FED.Id_ED LEFT JOIN 
		 [Mtro].[Distrito] AS MD ON DC.EM_UBIGEO = MD.CodigoUbigeo
	WHERE
		(DC.NUM_DOCUMENTO = @NroDocumento) AND
		(CTP.Id_TD = @TipoDocumento) AND
		(DC.SERIE = @Serie) AND
		(DC.EM_NUM_DOCU = @RucEmpresa)
END
GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ObtenerDocumentoXML]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ObtenerDocumentoXML]
	@DocumentName [varchar](30),
	@Estado [int] output
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON

DECLARE @Ti  VARCHAR(2);
DECLARE @TD VARCHAR(2);
SET @Ti  = SUBSTRING(@DocumentName,13,1);
SET @TD = SUBSTRING(@DocumentName,13,2);

IF (@Ti = 'R')
   BEGIN
   IF (@TD = 'RC')
      BEGIN
      IF NOT EXISTS (SELECT [NUM_CPE]
                     FROM   [Fact].[O.RBoletasCabecera] AS DC
                     WHERE  (DC.NUM_CPE = @DocumentName))
         BEGIN
            SET @Estado = 0
         END
      ELSE IF NOT EXISTS (SELECT [NUM_CPE]
                     FROM   [Fact].[O.RBoletasCabecera] AS DC
                     WHERE  (DC.NUM_CPE = @DocumentName) AND
                           (DC.VAR_RES IS NULL))
         BEGIN
            SET @Estado = 1
         END
      ELSE
         BEGIN
            SET NOCOUNT ON;
            SET @Estado = 2
            
            SELECT
               DC.ID_RBC,
               DC.[VAR_FIR],
               DC.DOC_TCK
            FROM [Fact].[O.RBoletasCabecera] AS DC
            WHERE
               (DC.NUM_CPE = @DocumentName) AND
               (DC.VAR_RES IS NULL)
         END
      END
   ELSE
      BEGIN
      IF NOT EXISTS (SELECT [NUM_CPE]
                     FROM   [Fact].[O.RBajasCabecera] AS DC
                     WHERE  (DC.NUM_CPE = @DocumentName))
         BEGIN
            SET @Estado = 0
         END
      ELSE IF NOT EXISTS (SELECT [NUM_CPE]
                     FROM   [Fact].[O.RBajasCabecera] AS DC
                     WHERE  (DC.NUM_CPE = @DocumentName) AND
                           (DC.VAR_RES IS NULL))
         BEGIN
            SET @Estado = 1
         END
      ELSE
         BEGIN
            SET NOCOUNT ON;
            SET @Estado = 2
            
            SELECT
               DC.ID_RAC,
               DC.[VAR_FIR],
               DC.DOC_TCK
            FROM [Fact].[O.RBajasCabecera] AS DC
            WHERE
               (DC.NUM_CPE = @DocumentName) AND
               (DC.VAR_RES IS NULL)
         END
      END
   END
ELSE
   BEGIN
   IF NOT EXISTS (SELECT [NUM_CPE]
      			   FROM   [Fact].[O.DocumentoCabecera] AS DC
      			   WHERE  (DC.NUM_CPE = @DocumentName))
   	BEGIN
   		SET @Estado = 0
   	END
   ELSE IF NOT EXISTS (SELECT [NUM_CPE]
         					FROM   [Fact].[O.DocumentoCabecera] AS DC
         					WHERE  (DC.NUM_CPE = @DocumentName) AND
   						   (DC.VAR_RES IS NULL))
   	BEGIN
   		SET @Estado = 1
   	END
   ELSE
   	BEGIN
         SET NOCOUNT ON;
   	   SET @Estado = 2
   
         SELECT
            DC.Id_DC,
   			DC.[VAR_FIR],
            DC.IMPRESORA
   		FROM [Fact].[O.DocumentoCabecera] AS DC
   		WHERE
   			(DC.NUM_CPE = @DocumentName) AND
   			(DC.VAR_RES IS NULL)
   	END
   END

   
--IF @T = 'R'
--   IF @TD = 'RC'
--
--   ELSE
--
--ELSE IF
--   IF NOT EXISTS (SELECT [NUM_CPE]
--   			   FROM   [Fact].[O.DocumentoCabecera] AS DC
--   			   WHERE  (DC.NUM_CPE = @DocumentName))
--   	BEGIN
--   		SET @Estado = 0
--   	END
--   ELSE IF NOT EXISTS (SELECT [NUM_CPE]
--   					FROM   [Fact].[O.DocumentoCabecera] AS DC
--   					WHERE  (DC.NUM_CPE = @DocumentName) AND
--   						   (DC.VAR_RES IS NULL))
--   	BEGIN
--   		SET @Estado = 1
--   	END
--   ELSE
--   	BEGIN
--         SET NOCOUNT ON;
--   	   SET @Estado = 2
--   
--         SELECT
--            DC.Id_DC,
--   			DC.[VAR_FIR],
--            DC.IMPRESORA
--   		FROM [Fact].[O.DocumentoCabecera] AS DC
--   		WHERE
--   			(DC.NUM_CPE = @DocumentName) AND
--   			(DC.VAR_RES IS NULL)
--   	END
--   GO

GO
/****** Object:  StoredProcedure [Fact].[O.Usp_ObtenerFechaDocumento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[O.Usp_ObtenerFechaDocumento] --1, 'FF11', '00000001', '20547025319'
	@TipoDocumento [int],
	@Serie [varchar](4),
	@NumeroDocumento [varchar](20),
	@RucEmpresa [varchar](11)
WITH EXECUTE AS CALLER
AS	
--BEGIN
--DECLARE @Serie VARCHAR(4) SET @Serie = 'B001'

--SELECT @LET


DECLARE @LET VARCHAR(4) SET @LET = SUBSTRING(@Serie, 1, 1)

 IF(@LET = 'F')
	BEGIN

	SELECT
			FD.[SERIE] AS Serie,
			FD.[FEC_EMIS] AS FechaEmision,
	 		CONVERT(VARCHAR(50), FD.[TOT_IMPOR_TOTAL]) AS MontoTotal,
			CTD.Code AS CodigoDocumento
		FROM [Fact].[O.DocumentoCabecera] AS FD INNER JOIN
			 [Ctl].[TipoDocumento] AS CTP ON FD.[ID_TPO_CPE] = CTP.[Code] INNER JOIN
			 [Ctl].[TipoDocumento] AS CTD ON FD.ID_TPO_CPE = CTD.Code INNER JOIN
			 [Fact].[O.EstadoDocumento] AS ED ON FD.Id_ED = ED.Id_ED
		WHERE 
		    --(FD.Id_ED = 7) AND 
			(ED.Abrev = 'SOK') AND 
			(FD.[SERIE] = @Serie) AND
			(FD.[NUM_DOCUMENTO] = @NumeroDocumento) AND
			(CTP.[Id_TD] = @TipoDocumento) AND
			(FD.[EM_NUM_DOCU] = @RucEmpresa)
	END
		
ELSE
	BEGIN
	SELECT
			FD.[SERIE] AS Serie,
			FD.[FEC_EMIS] AS FechaEmision,
	 		CONVERT(VARCHAR(50), FD.[TOT_IMPOR_TOTAL]) AS MontoTotal,
			CTD.Code AS CodigoDocumento
		FROM [Fact].[O.DocumentoCabecera] AS FD INNER JOIN
			 [Ctl].[TipoDocumento] AS CTP ON FD.[ID_TPO_CPE] = CTP.[Code] INNER JOIN
			 [Ctl].[TipoDocumento] AS CTD ON FD.ID_TPO_CPE = CTD.Code
		WHERE 
			(FD.[SERIE] = @Serie) AND
			(FD.[NUM_DOCUMENTO] = @NumeroDocumento) AND
			(CTP.[Id_TD] = @TipoDocumento) AND
			(FD.[EM_NUM_DOCU] = @RucEmpresa)
	END

GO
/****** Object:  StoredProcedure [Fact].[Usp_EstadoSistema_Error]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_EstadoSistema_Error] --'20508622326', '27-03-2017'
@RucEntity VARCHAR(11),
@fechtoday DATETIME
AS
BEGIN
	DECLARE @TableInicio TABLE (Criterio varchar(100), Hoy int, Ayer int, UltimaSemana int, TotalMes int, MesPasado int, Codigo varchar(20))

	DECLARE @Ayer DATETIME SET @Ayer = CONVERT(DATETIME, CONVERT(VARCHAR(12), CONVERT(DATETIME, DATEADD(DAY, -1, GETDATE()))),112) 
	DECLARE @primerdiasempas DATETIME SET @primerdiasempas = CONVERT(DATETIME, CONVERT(VARCHAR(12),  dateadd(week, datediff(week, 0, getdate()), -7)),112)
	DECLARE @ultimodiasempas DATETIME SET @ultimodiasempas = CONVERT(DATETIME, CONVERT(VARCHAR(12), dateadd(week, datediff(week, 0, getdate()), -1)),112)
	DECLARE @primerdiamesactual DATETIME SET @primerdiamesactual = CONVERT(DATETIME, CONVERT(VARCHAR(12), DATEADD(dd,-(DAY(getdate())-1),getdate())),112) 
	DECLARE @ultimodiadiamesactual DATETIME SET @ultimodiadiamesactual = CONVERT(DATETIME, CONVERT(VARCHAR(12), DATEADD(dd,-(DAY(DATEADD(mm,1,getdate()))),DATEADD(mm,1,getdate()))),112)
	DECLARE @primerdiamespasado DATETIME SET @primerdiamespasado = CONVERT(DATETIME, CONVERT(VARCHAR(12), DATEADD(month, DATEDIFF(month, 0, DATEADD(month, -1,GETDATE())), 0)),112)
	DECLARE @ultimodiamespasado DATETIME SET @ultimodiamespasado = CONVERT(DATETIME, CONVERT(VARCHAR(12), DATEADD(dd,-(DAY(GETDATE())),GETDATE())),112)

	DECLARE @Tot_Hoy INT SET @Tot_Hoy = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([FEC_EMIS] BETWEEN @fechtoday AND @fechtoday ) AND [EM_NUM_DOCU] = @RucEntity)
	DECLARE @Tot_Ayer INT SET @Tot_Ayer = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([FEC_EMIS] = @Ayer) AND [EM_NUM_DOCU] = @RucEntity)
	DECLARE @Tot_UltimaSemana INT SET @Tot_UltimaSemana = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([FEC_EMIS] BETWEEN @primerdiasempas AND @ultimodiasempas ) AND [EM_NUM_DOCU] = @RucEntity)
	DECLARE @Tot_Mes INT SET @Tot_Mes = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([FEC_EMIS] BETWEEN @primerdiamesactual AND @ultimodiadiamesactual) AND [EM_NUM_DOCU] = @RucEntity)
	DECLARE @Tot_MesPasado INT SET @Tot_MesPasado = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([FEC_EMIS] BETWEEN @primerdiamespasado AND @ultimodiamespasado) AND [EM_NUM_DOCU] = @RucEntity)

	INSERT INTO @TableInicio VALUES ('Total Doc Electrónicos'
										, @Tot_Hoy
										, @Tot_Ayer
										, @Tot_UltimaSemana
										, @Tot_Mes
										, @Tot_MesPasado
										, ('c-ce'))

	--SET @Tot_Ayer = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = 2) AND ([FEC_EMIS] = @Ayer) AND [EM_NUM_DOCU] = @RucEntity)
	--SET @Tot_UltimaSemana = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = 2) AND ([FEC_EMIS] BETWEEN @primerdiasempas AND @ultimodiasempas ) AND [EM_NUM_DOCU] = @RucEntity)
	--SET @Tot_Mes = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = 2) AND ([FEC_EMIS] BETWEEN @primerdiamesactual AND @ultimodiadiamesactual) AND [EM_NUM_DOCU] = @RucEntity)
	--SET @Tot_MesPasado = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = 2) AND ([FEC_EMIS] BETWEEN @primerdiamespasado AND @ultimodiamespasado) AND [EM_NUM_DOCU] = @RucEntity)

	--INSERT INTO @TableInicio VALUES ('Procesamiento CE'
	--									, @Tot_Ayer
	--									, @Tot_UltimaSemana
	--									, @Tot_Mes
	--									, @Tot_MesPasado
	--									, ('p-ce'))

	--SET @Tot_Ayer = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = 3) AND ([FEC_EMIS] = @Ayer) AND [EM_NUM_DOCU] = @RucEntity)
	--SET @Tot_UltimaSemana = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = 3) AND ([FEC_EMIS] BETWEEN @primerdiasempas AND @ultimodiasempas ) AND [EM_NUM_DOCU] = @RucEntity)
	--SET @Tot_Mes = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = 3) AND ([FEC_EMIS] BETWEEN @primerdiamesactual AND @ultimodiadiamesactual) AND [EM_NUM_DOCU] = @RucEntity)
	--SET @Tot_MesPasado = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = 3) AND ([FEC_EMIS] BETWEEN @primerdiamespasado AND @ultimodiamespasado) AND [EM_NUM_DOCU] = @RucEntity)

	--INSERT INTO @TableInicio VALUES ('Enviado SUNAT'
	--									, @Tot_Ayer
	--									, @Tot_UltimaSemana
	--									, @Tot_Mes
	--									, @Tot_MesPasado
	--									, ('e-snt'))

	DECLARE @Id_ED INT 
	SET @Id_ED = (SELECT Id_ED FROM [Fact].[O.EstadoDocumento] WHERE Abrev = 'EEN')

	SET @Tot_Hoy = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @fechtoday AND @fechtoday ) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_Ayer = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] = @Ayer) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_UltimaSemana = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiasempas AND @ultimodiasempas ) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_Mes = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamesactual AND @ultimodiadiamesactual) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_MesPasado = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamespasado AND @ultimodiamespasado) AND [EM_NUM_DOCU] = @RucEntity)

	INSERT INTO @TableInicio VALUES ('Error de Envio'
										, @Tot_Hoy
										, @Tot_Ayer
										, @Tot_UltimaSemana
										, @Tot_Mes
										, @Tot_MesPasado
										, ('er-snt'))

	--DECLARE @Id_ED INT 
	SET @Id_ED = (SELECT Id_ED FROM [Fact].[O.EstadoDocumento] WHERE Abrev = 'XGN')

	SET @Tot_Hoy = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @fechtoday AND @fechtoday ) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_Ayer = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] = @Ayer) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_UltimaSemana = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiasempas AND @ultimodiasempas ) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_Mes = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamesactual AND @ultimodiadiamesactual) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_MesPasado = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamespasado AND @ultimodiamespasado) AND [EM_NUM_DOCU] = @RucEntity)

	INSERT INTO @TableInicio VALUES ('Xml Generado'
										, @Tot_Hoy
										, @Tot_Ayer
										, @Tot_UltimaSemana
										, @Tot_Mes
										, @Tot_MesPasado
										, ('xml-gn'))

	SELECT
		Criterio
		,Hoy
		,Ayer
		,UltimaSemana
		,TotalMes
		,MesPasado
		, Codigo
	FROM @TableInicio

END

GO
/****** Object:  StoredProcedure [Fact].[Usp_EstadoSistema_Ok]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_EstadoSistema_Ok] --'20508622326', '27-03-2017'
@RucEntity VARCHAR(11),
@fechtoday DATE
AS
BEGIN
	DECLARE @TableInicio TABLE (Criterio varchar(100), Hoy INT, Ayer int, UltimaSemana int, TotalMes int, MesPasado int, Codigo varchar(20))

	DECLARE @Ayer DATETIME SET @Ayer = CONVERT(DATETIME, CONVERT(VARCHAR(12), CONVERT(DATETIME, DATEADD(DAY, -1, GETDATE()))),112) 
	DECLARE @primerdiasempas DATETIME SET @primerdiasempas = CONVERT(DATETIME, CONVERT(VARCHAR(12),  dateadd(week, datediff(week, 0, getdate()), -7)),112)
	DECLARE @ultimodiasempas DATETIME SET @ultimodiasempas = CONVERT(DATETIME, CONVERT(VARCHAR(12), dateadd(week, datediff(week, 0, getdate()), -1)),112)
	DECLARE @primerdiamesactual DATETIME SET @primerdiamesactual = CONVERT(DATETIME, CONVERT(VARCHAR(12), DATEADD(dd,-(DAY(getdate())-1),getdate())),112) 
	DECLARE @ultimodiadiamesactual DATETIME SET @ultimodiadiamesactual = CONVERT(DATETIME, CONVERT(VARCHAR(12), DATEADD(dd,-(DAY(DATEADD(mm,1,getdate()))),DATEADD(mm,1,getdate()))),112)
	DECLARE @primerdiamespasado DATETIME SET @primerdiamespasado = CONVERT(DATETIME, CONVERT(VARCHAR(12), DATEADD(month, DATEDIFF(month, 0, DATEADD(month, -1,GETDATE())), 0)),112)
	DECLARE @ultimodiamespasado DATETIME SET @ultimodiamespasado = CONVERT(DATETIME, CONVERT(VARCHAR(12), DATEADD(dd,-(DAY(GETDATE())),GETDATE())),112)


	DECLARE @Id_ED INT 
	SET @Id_ED = (SELECT Id_ED FROM [Fact].[O.EstadoDocumento] WHERE Abrev = 'SOK')


	DECLARE @Tot_Hoy INT SET @Tot_Hoy = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @fechtoday AND @fechtoday ) AND [EM_NUM_DOCU] = @RucEntity )
	DECLARE @Tot_Ayer INT SET @Tot_Ayer = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] = @Ayer) AND [EM_NUM_DOCU] = @RucEntity )
	DECLARE @Tot_UltimaSemana INT SET @Tot_UltimaSemana = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiasempas AND @ultimodiasempas ) AND [EM_NUM_DOCU] = @RucEntity)
	DECLARE @Tot_Mes INT SET @Tot_Mes = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamesactual AND @ultimodiadiamesactual) AND [EM_NUM_DOCU] = @RucEntity)
	DECLARE @Tot_MesPasado INT SET @Tot_MesPasado = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamespasado AND @ultimodiamespasado) AND [EM_NUM_DOCU] = @RucEntity)

	INSERT INTO @TableInicio VALUES ('Aceptados Ok'
										, @Tot_Hoy
										, @Tot_Ayer
										, @Tot_UltimaSemana
										, @Tot_Mes
										, @Tot_MesPasado
										, ('Ok'))

	SET @Id_ED = (SELECT Id_ED FROM [Fact].[O.EstadoDocumento] WHERE Abrev = 'SOB')

	SET @Tot_Hoy = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @fechtoday AND @fechtoday ) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_Ayer = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] = @Ayer) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_UltimaSemana = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiasempas AND @ultimodiasempas ) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_Mes = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamesactual AND @ultimodiadiamesactual) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_MesPasado = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamespasado AND @ultimodiamespasado) AND [EM_NUM_DOCU] = @RucEntity)

	INSERT INTO @TableInicio VALUES ('Aceptados con Obsv.'
										, @Tot_Hoy
										, @Tot_Ayer
										, @Tot_UltimaSemana
										, @Tot_Mes
										, @Tot_MesPasado
										, ('Atdo-Obs'))

	SET @Id_ED = (SELECT Id_ED FROM [Fact].[O.EstadoDocumento] WHERE Abrev = 'SRE')

	SET @Tot_Hoy = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @fechtoday AND @fechtoday ) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_Ayer = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] = @Ayer) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_UltimaSemana = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiasempas AND @ultimodiasempas ) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_Mes = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamesactual AND @ultimodiadiamesactual) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_MesPasado = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamespasado AND @ultimodiamespasado) AND [EM_NUM_DOCU] = @RucEntity)

	INSERT INTO @TableInicio VALUES ('Rechazados'
										, @Tot_Hoy
										, @Tot_Ayer
										, @Tot_UltimaSemana
										, @Tot_Mes
										, @Tot_MesPasado
										, ('Rechaz'))


	SET @Id_ED = (SELECT Id_ED FROM [Fact].[O.EstadoDocumento] WHERE Abrev = 'ANS')

	SET @Tot_Hoy = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @fechtoday AND @fechtoday ) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_Ayer = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] = @Ayer) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_UltimaSemana = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiasempas AND @ultimodiasempas ) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_Mes = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamesactual AND @ultimodiadiamesactual) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_MesPasado = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamespasado AND @ultimodiamespasado) AND [EM_NUM_DOCU] = @RucEntity)

	INSERT INTO @TableInicio VALUES ('Anul. Pend. de Envío'
										, @Tot_Hoy
										, @Tot_Ayer
										, @Tot_UltimaSemana
										, @Tot_Mes
										, @Tot_MesPasado
										, ('an-pnd'))


	SET @Id_ED = (SELECT Id_ED FROM [Fact].[O.EstadoDocumento] WHERE Abrev = 'AAS')

	SET @Tot_Hoy = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @fechtoday AND @fechtoday ) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_Ayer = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] = @Ayer) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_UltimaSemana = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiasempas AND @ultimodiasempas ) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_Mes = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamesactual AND @ultimodiadiamesactual) AND [EM_NUM_DOCU] = @RucEntity)
	SET @Tot_MesPasado = (SELECT COUNT([Id_DC]) FROM [Fact].[O.DocumentoCabecera] WHERE ([Id_ED] = @Id_ED) AND ([FEC_EMIS] BETWEEN @primerdiamespasado AND @ultimodiamespasado) AND [EM_NUM_DOCU] = @RucEntity)

	INSERT INTO @TableInicio VALUES ('Anul. Aceptado Sunat'
										, @Tot_Hoy
										, @Tot_Ayer
										, @Tot_UltimaSemana
										, @Tot_Mes
										, @Tot_MesPasado
										, ('an-ok'))


	SELECT * FROM @TableInicio
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_ExisteDocAnualdo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_ExisteDocAnualdo]
	@IdTipoDocumento [int],
	@Serie [varchar](4),
	@NumeroDocumento [varchar](20),
	@RucEmpresa [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
			[Id_DocAnul] AS Nro,
			[Serie] AS Serie,
			[Correlativo] AS NumeroDocumento,
			--CONVERT(VARCHAR(30), [Fecha_Anul], 111) AS FechaAnulado,
			[Fecha_Anul] AS FechaAnulado
		FROM [Fact].[O.Documento_Anul]
		WHERE ([Id_TpoDoc] = @IdTipoDocumento) AND
			  ([Serie] = @Serie) AND
			  ([Correlativo] = @NumeroDocumento) AND
			  --(CONVERT(INT, [Correlativo]) = CONVERT(INT, @NumeroDocumento)) AND
			  ([RucEmpresa] = @RucEmpresa)
			  
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_ExisteDocAnualdoWS]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_ExisteDocAnualdoWS]
@NUM_CE VARCHAR(45),
@TIPO_CE VARCHAR(2)
AS
BEGIN
	SELECT 
		CTP.[Id_TD] IdTpoDoc,
		DC.[SERIE] AS Serie,
		DC.[NUM_DOCUMENTO] AS NumeroDocumento,
		DC.[FEC_EMIS] AS FechaAnulado
	FROM [Ctl].[TipoDocumento] AS CTP INNER JOIN
		 [Fact].[O.DocumentoCabecera] AS DC ON CTP.[Code] = DC.[ID_TPO_CPE]
	WHERE [NUM_CPE] = @NUM_CE AND
		  CTP.[Code] = @TIPO_CE
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_ExisteDocWS]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_ExisteDocWS]
@NUM_CE VARCHAR(45),
@TIPO_CE VARCHAR(2)
AS
BEGIN
	SELECT 
		CTP.[Id_TD] IdTpoDoc,
		DC.[SERIE] AS Serie,
		DC.[NUM_DOCUMENTO] AS NumeroDocumento,
		DC.[FEC_EMIS] AS FechaEmision
	FROM [Ctl].[TipoDocumento] AS CTP INNER JOIN
		 [Fact].[O.DocumentoCabecera] AS DC ON CTP.[Code] = DC.[ID_TPO_CPE]
	WHERE [NUM_CPE] = @NUM_CE AND
		  CTP.[Code] = @TIPO_CE
END
GO
/****** Object:  StoredProcedure [Fact].[Usp_GetAmbDoc]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_GetAmbDoc] --'01', '20508622326'
   @DocumentType [varchar](2),
   @RUC [varchar](11)
AS
SET NOCOUNT ON
BEGIN
   Declare @TempTableVariable 
   TABLE(
      COD varchar(50),
      USR VARCHAR(250),
      PWD VARCHAR(MAX),
      MUR VARCHAR(MAX),
      QUR VARCHAR(MAX)
   );

   DECLARE @IDAMB VARCHAR(11);
   DECLARE @CCOD VARCHAR(50);
   DECLARE @CUSR VARCHAR(250);
   DECLARE @CPWD VARCHAR(MAX);
   DECLARE @CMUR VARCHAR(MAX);
   DECLARE @CQUR VARCHAR(MAX);

   SET @IDAMB = (SELECT TOP 1 IDAMBIENTE FROM Conf.AmbienteTrabj WHERE RUCENTITY = @RUC)
   SET @CCOD  = (SELECT TOP 1 COD FROM Conf.AmbienteTrabj WHERE RUCENTITY = @RUC)
   SET @CUSR  = (SELECT C.[NombreUsuario] FROM [Fact].[CredentialsCertificateAmb] AS C WHERE C.IDAMBIENTE = @IDAMB AND C.RUCENTITY = @RUC)
   SET @CPWD  = (SELECT C.[Password] FROM [Fact].[CredentialsCertificateAmb] AS C WHERE C.IDAMBIENTE = @IDAMB AND C.RUCENTITY = @RUC)
   SET @CMUR  = (SELECT U.[URL] FROM [Conf].[URL_SUNAT] AS U WHERE U.IDAMBIENTE = @IDAMB AND U.DOCUMENTOS LIKE '%' + @DocumentType + +'%')
   SET @CQUR  = (SELECT U.[URL] FROM [Conf].[URL_SUNAT] AS U WHERE U.IDAMBIENTE = @IDAMB AND U.DOCUMENTOS = 'Q')
   INSERT INTO @TempTableVariable (COD, USR, PWD, MUR, QUR)
   VALUES (ISNULL(@CCOD,''), ISNULL(@CUSR,''), ISNULL(@CPWD,''), ISNULL(@CMUR,''), ISNULL(@CQUR,''))

	SELECT * FROM @TempTableVariable
--   SELECT
--      A.[COD] COD,
--      C.[NombreUsuario] AS 'USR',
--      C.[Password] PWD
--   FROM [Conf].[Tpo_Amb_Trabj] AS A 
--   INNER JOIN [Conf].[DocumentoAmbiente] AS B ON A.IDAMBIENTE = B.IDAMBIENTE
--   INNER JOIN [Fact].[CredentialsCertificateAmb] AS C ON A.IDAMBIENTE = C.IDAMBIENTE
--   WHERE B.TIPO_CE = @DocumentType
--      AND B.IDESTADO = 1
--      AND B.RUCENTITY = @RUC
--      AND C.RUCENTITY = @RUC
--      AND B.IDAMBIENTE = @IDAMB
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_GetCredencialEntity_Received]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_GetCredencialEntity_Received] --'20101071562'
@RucEmpresa VARCHAR(11)
AS
BEGIN
	SELECT
		ME.[Ruc],
		SC.[Email],
		SC.[Password],
		SC.[DOMAIN] AS Domain,
		SC.[IP] AS IP,
		SC.[PORT] AS Port,
		SC.[UseSSL] AS UseSSL
	FROM [Seg].[Correo] AS SC INNER JOIN
		 [Mtro].[Empresa] AS ME ON SC.IdEmpresa = ME.IdEmpresa
	WHERE ME.Ruc = @RucEmpresa AND SC.[IdEstado] = 1 AND SC.TypeMail = 'Reception'
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_GetCredencialEntitySend]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_GetCredencialEntitySend] --'20547025319'
@RucEmpresa VARCHAR(11)
AS
DECLARE @CORR VARCHAR(MAX);
SET @CORR = (SELECT STUFF((SELECT Correos + ';' FROM [Conf].[MailNotifications] WHERE RucEntity = @RucEmpresa AND TypeMail = 'f' ORDER BY correos FOR XML PATH(''), TYPE).value('(./text())[1]','varchar(max)'), 1, 0, '') AS maillist)
BEGIN
	SELECT
		ME.[Ruc],
		SC.[Email],
		SC.[Password],
		SC.[DOMAIN] AS Domain,
		SC.[IP] AS IP,
		SC.[PORT] AS Port,
		ISNULL(SC.[UseSSL],0) AS UseSSL,
		ISNULL(@CORR,'') AS MailNotif,
		ISNULL(ME.Url_CompanyLogo,'') AS Url_CompanyLogo,
		ISNULL(ME.Url_CompanyConsult,'') AS Url_CompanyConsult
	FROM [Seg].[Correo] AS SC INNER JOIN
		 [Mtro].[Empresa] AS ME ON SC.RucEmpresa = ME.[Ruc]
	WHERE ME.Ruc = @RucEmpresa AND SC.[IdEstado] = 1 AND SC.TypeMail = 'Send'
END
GO
/****** Object:  StoredProcedure [Fact].[Usp_GetCredencialEntitySendServices]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_GetCredencialEntitySendServices] --'20508622326'
@RucEmpresa VARCHAR(11)
AS
DECLARE @CORR VARCHAR(MAX);
SET @CORR = (SELECT STUFF((SELECT Correos + ';' FROM [Conf].[MailNotifications] WHERE TypeMail = 'S' ORDER BY correos FOR XML PATH(''), TYPE).value('(./text())[1]','varchar(max)'), 1, 0, '') AS maillist)
BEGIN
	SELECT
		ME.[Ruc],
		SC.[Email],
		SC.[Password],
		SC.[DOMAIN] AS Domain,
		SC.[IP] AS IP,
		SC.[PORT] AS Port,
		ISNULL(SC.[UseSSL],0) AS UseSSL,
		ISNULL(@CORR,'') AS MailNotif,
		ISNULL(ME.Url_CompanyLogo,'') AS Url_CompanyLogo,
		ISNULL(ME.Url_CompanyConsult,'') AS Url_CompanyConsult
	FROM [Seg].[Correo] AS SC INNER JOIN
		 [Mtro].[Empresa] AS ME ON SC.IdEmpresa = ME.IdEmpresa
	WHERE SC.[IdEstado] = 1 AND SC.TypeMail = 'Support'
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_GetDocCabeceraCRECPE]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_GetDocCabeceraCRECPE]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		[Code] AS TipoDocRelac,
		[ID_CPE] AS NroDocRelac,
		[FEC_EMIS] AS FechaEmisionDocRelac,
		[TPO_MONEDA] AS MonedaImpTotDocRelac,
		[TOT_IMPOR_TOTAL] AS ImporteTotDocRelac,
		[FECHATC] AS FechaPago,
		[IT_NRO_ORD] AS NumeroPago,
		[IT_VAL_VTA] AS ImportePagoSinReten,
		[IMPTOT] AS ImporteRetenido,
		[IMPTOTCE] AS ImporteTotxPagoNeto
		,'' AS Simbolo
		,'' AS SimboloSol
	FROM [Ctl].[TipoDocumento] AS CTP INNER JOIN
		 [Fact].[O.DocumentoCabecera] AS FDC ON CTP.Code = FDC.ID_TPO_CPE INNER JOIN
		 [Fact].[O.DocumentoDetalle]AS FDD ON FDC.[Id_DC] = FDD.[Id_DC]
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_GetDocumentAfectado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_GetDocumentAfectado]
AS
BEGIN
	SELECT 
		[DOC_AFEC_ID] AS ID,
		[ID_TPO_CPE] AS CodigoTipoDocumento
	FROM [Fact].[O.Afectado]
END
GO
/****** Object:  StoredProcedure [Fact].[Usp_GetDocumentoXML]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_GetDocumentoXML] --'20431084172-20-R001-00000016'
@NUM_CPE VARCHAR(40)
AS
BEGIN
	SELECT
		[ID_TPO_CPE] AS TPO_CPE,
		ISNULL([VAR_FIR], 0) AS 'XML',
		CONVERT(VARCHAR(MAX), [VAR_FIR]) AS 'XML2',
		--CONVERT(VARCHAR(1), (ISNULL(CAMPO2, '0'))) AS TypeFormat
		'2' AS TypeFormat
	FROM [Fact].[O.DocumentoCabecera]
	WHERE [NUM_CPE] = @NUM_CPE --AND
		  --[Id_ED] in (2,4,7)
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_GetEmailCliente]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_GetEmailCliente]
@NroDocumento VARCHAR(11)
AS
BEGIN
	SELECT 
		[NroDocumento],
		[RazonSocial],
		[Email]
	FROM [Mtro].[Cliente]
	WHERE [NroDocumento] = @NroDocumento
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_GetExchangeRate_TD]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_GetExchangeRate_TD]-- '2017-01-11'
@Fecha varchar(10)
AS
BEGIN
	SELECT
		Fecha,
		Moneda,
		[Cambio] AS Value
	FROM [Fact].[ExchangeRate]
	WHERE [Fecha] = CONVERT(datetime, @Fecha, 20 )
	--WHERE [Fecha] = @Fecha
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_GetList_PendingDocuments_ErrorSend]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [Fact].[Usp_GetList_PendingDocuments_ErrorSend] --'20547025319'
@ruccomp VARCHAR(11)
AS
BEGIN
	SELECT
		FEC_EMIS AS FechaEmision, ED.[Desc] AS Estado, COUNT(*) AS Cantidad
	FROM [Fact].[O.DocumentoCabecera] AS DC INNER JOIN
		 [Fact].[O.EstadoDocumento] AS ED ON DC.Id_ED = ED.Id_ED
	WHERE EM_NUM_DOCU = @ruccomp AND ED.Abrev IN('EEN')
	GROUP BY DC.FEC_EMIS, ED.[Desc]

END
GO
/****** Object:  StoredProcedure [Fact].[Usp_GetList_PendingDocuments_RA]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_GetList_PendingDocuments_RA] --'20547025319'
@ruccomp VARCHAR(11)
AS
BEGIN
	SELECT
		FEC_EMIS AS FechaEmision, ED.[Desc] AS Estado, COUNT(*) AS Cantidad
	FROM [Fact].[O.DocumentoCabecera] AS DC INNER JOIN
		 [Fact].[O.EstadoDocumento] AS ED ON DC.Id_ED = ED.Id_ED
	--WHERE EM_NUM_DOCU = @ruccomp AND ED.Abrev NOT IN('SOK', 'SOB', 'SRE', 'AAS') AND ED.Abrev NOT IN('XGN', 'EEN')
	WHERE EM_NUM_DOCU = @ruccomp AND ED.Abrev IN('ANS')
	GROUP BY DC.FEC_EMIS, ED.[Desc]
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_GetList_PendingDocuments_RC]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_GetList_PendingDocuments_RC] --'20547025319'
@ruccomp VARCHAR(11)
AS
BEGIN
	SELECT
		FEC_EMIS AS FechaEmision, ED.[Desc] AS Estado, COUNT(*) AS Cantidad
	FROM [Fact].[O.DocumentoCabecera] AS DC INNER JOIN
		 [Fact].[O.EstadoDocumento] AS ED ON DC.Id_ED = ED.Id_ED
	--WHERE EM_NUM_DOCU = @ruccomp AND ED.Abrev NOT IN('SOK', 'SOB', 'SRE', 'AAS') AND ED.Abrev NOT IN('EEN', 'AES')
	WHERE EM_NUM_DOCU = @ruccomp AND ED.Abrev IN('XGN') --AND ED.Abrev NOT IN('SOK', 'SOB', 'SRE', 'AAS')
	GROUP BY DC.FEC_EMIS, ED.[Desc]

END

GO
/****** Object:  StoredProcedure [Fact].[Usp_GetListCredentialCertificateAmb]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_GetListCredentialCertificateAmb]
@RUC VARCHAR(11),
@IDAMBIENTE INT
AS
BEGIN
	SELECT
		FCC.[ID],
		FCC.[RucEntity] AS 'RucEmpresa',
		FCC.[NombreUsuario] AS 'Username',
		FCC.[Password],
		FCC.[IDAMBIENTE],
		CTA.[COD] AS Codigo
	FROM [Conf].[Tpo_Amb_Trabj] AS CTA INNER JOIN
		 [Fact].[CredentialsCertificateAmb] AS FCC ON CTA.IDAMBIENTE = FCC.IDAMBIENTE
	WHERE FCC.RucEntity = @RUC AND FCC.IdAmbiente = @IDAMBIENTE
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_GetStatusDocument]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_GetStatusDocument]
@NUM_CE VARCHAR(30)
AS
BEGIN
	SELECT 
		DC.[NUM_CPE] AS NUM_CE,
		--DC.[Id_ED] AS IdEstado,
		ED.Abrev AS Cod,
		ED.[Desc] AS Estado
	FROM [Fact].[O.EstadoDocumento] AS ED INNER JOIN
	     [Fact].[O.DocumentoCabecera] AS DC ON ED.Id_ED = DC.Id_ED
	WHERE DC.[NUM_CPE] = @NUM_CE
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_IfExistsDocument]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_IfExistsDocument]
	@NUM_CPE [varchar](50)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		[Id_DC] AS Nro 
	FROM [Fact].[I.DocumentoCabecera]
	WHERE [NUM_CPE] = @NUM_CPE
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_Ins_ExchangeRate_Today]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_Ins_ExchangeRate_Today] --'2017-02-03 00:00:00.000', '03-02-2017', 4.1234
@fech datetime,
@fecf_str varchar(10),
@cambio decimal(10,4) 
as
IF NOT EXISTS (SELECT [Cambio] FROM [Fact].[ExchangeRate] WHERE [fech_str] = @fecf_str)
begin
	insert into [Fact].[ExchangeRate]
		(RucNumber, Fecha, Moneda, Cambio, fech_str)
		values
		('', @fech, 'USD', @cambio, @fecf_str)
		--insert into MBTCA (MbTcaFec, MbMonCod, MbTcaVal)
		--	values (@fech, 'USD', @value)
			--values (@fecf_str, 'USD', @value)

end
ELSE
begin
	update [Fact].[ExchangeRate] set [Cambio] = @cambio where [fech_str] = @fecf_str
	--update MBTCA set MbTcaVal = @value where MbTcaFec = @fecf_str
	--select CONVERT(datetime, CONVERT(VARCHAR(10), @fech) + ' 00:00:00.000')


end
GO
/****** Object:  StoredProcedure [Fact].[Usp_Insert_ConfigMain]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_Insert_ConfigMain]
@TAB varchar(30),
@NOM varchar(30),
@POS varchar(3),
@VAL varchar(150),
@MND varchar(1),
@DOC varchar(30),
@MSG varchar(150),
@ECV varchar(50),
@ECN varchar(50)
as
begin
	insert into [Fact].[CONFIG_MAIN]
		values (@TAB,
				@NOM,
				@POS,
				@VAL,
				@MND,
				@DOC,
				@MSG,
				@ECV,
				@ECN)
	end

GO
/****** Object:  StoredProcedure [Fact].[Usp_InsertarDocumentoEnviado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_InsertarDocumentoEnviado]
@IdTipoDocumento INT,
@TPO_CE VARCHAR(2),
@Serie VARCHAR(4),
@Correlativo VARCHAR(15),
@Destino VARCHAR(250),
@Asunto VARCHAR(250),
@Mensaje VARCHAR(250),
@Remitente VARCHAR(250),
@FechaEnvio DATETIME,
@Fecha_Cad VARCHAR(25),
@RucEmpresa VARCHAR(11)
AS
BEGIN
	INSERT INTO [Fact].[DocumentoEnviado]
		(IdTipoDocumento, Serie, Correlativo, Destino, 
		Asunto, Mensaje, Remitente, FechaEnvio, Fecha_Cad, RucEmpresa, TPO_CE)
		VALUES
		(@IdTipoDocumento, @Serie, @Correlativo, @Destino,
		@Asunto, @Mensaje, @Remitente, @FechaEnvio, @Fecha_Cad, @RucEmpresa, @TPO_CE)
END
GO
/****** Object:  StoredProcedure [Fact].[Usp_InsertDocumentAmbient]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_InsertDocumentAmbient]
@TIPO_CE VARCHAR(2),
@IDAMBIENTE INT,
@IDESTADO INT,
@RUCENTITY VARCHAR(11)
AS
IF NOT EXISTS (SELECT ID FROM [Conf].[DocumentoAmbiente] WHERE TIPO_CE = @TIPO_CE AND IDAMBIENTE = @IDAMBIENTE AND RUCENTITY = @RUCENTITY)
	BEGIN
		INSERT INTO [Conf].[DocumentoAmbiente]
			(TIPO_CE, IDAMBIENTE, IDESTADO, RUCENTITY)
			VALUES
			(@TIPO_CE, @IDAMBIENTE, @IDESTADO, @RUCENTITY)
	END
ELSE
	BEGIN
		UPDATE [Conf].[DocumentoAmbiente] SET
			--TIPO_CE = @TIPO_CE,
			--IDAMBIENTE = @IDAMBIENTE,
			IDESTADO = @IDESTADO
			--RUCENTITY = @RUCENTITY
		WHERE TIPO_CE = @TIPO_CE AND IDAMBIENTE = @IDAMBIENTE AND RUCENTITY = @RUCENTITY
	END
IF(@IDESTADO = 1)
BEGIN
	UPDATE [Conf].[DocumentoAmbiente]
		SET [IDESTADO] = 2
	WHERE [IDAMBIENTE] != @IDAMBIENTE AND
		  [TIPO_CE] = @TIPO_CE AND
		  [RUCENTITY] = @RUCENTITY
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_ListaDocumentoEnviado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [Fact].[Usp_ListaDocumentoEnviado] --'13/04/2016','20/04/2016','',0,'20216414056'
@FechaDesde VARCHAR(40),
@FechaHasta VARCHAR(40),
@Serie VARCHAR(4),
@TipoDocumento INT,
@RucEmpresa VARCHAR(11)
AS
BEGIN
	SELECT 
		--IdDocEnviado AS Nro,
		CONVERT(INT, ROW_NUMBER() OVER(ORDER BY IdDocEnviado)) AS Nro,
		TP.[Id_TD] AS IdTipoDocumento,
		TP.[Code] AS TipoDocumento,
		TP.[Desc] AS Descripcion,
		DE.Serie,
		DE.Correlativo AS NumeroDocumento,
		DE.Asunto,
		--DE.FechaEnvio,
		DE.Fecha_Cad AS FechaEnvio,
		DE.Destino,
		DE.Remitente
	FROM [Fact].[DocumentoEnviado] AS DE INNER JOIN
		 [Ctl].[TipoDocumento] AS TP ON DE.[TPO_CE] = TP.[Code]
	WHERE (@FechaDesde = '' AND @FechaHasta = '' OR DE.FechaEnvio BETWEEN @FechaDesde AND @FechaHasta) AND
		  (@Serie = '' OR DE.Serie = @Serie) AND
		  (@TipoDocumento = 0 OR TP.[Id_TD] = @TipoDocumento) AND
		  (DE.RucEmpresa = @RucEmpresa)
END
GO
/****** Object:  StoredProcedure [Fact].[Usp_ListaEstadoDocumento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_ListaEstadoDocumento]
WITH EXECUTE AS CALLER
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		0 AS IdEstado,
		'- Todos -' AS Descripcion
	UNION
	SELECT 
		[Id_ED] AS IdEstado,
		[Desc] AS Descripcion
	FROM [Fact].[O.EstadoDocumento]
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_ListaHistorialCliente]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_ListaHistorialCliente]
	@TipoDocumento [int],
	@RucEmpresa [varchar](11),
	@ClienteRuc_Dni [varchar](50),
	@FechaInicio [varchar](10),
	@FechaFin [varchar](10)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		--CONVERT(INT,RANK() OVER (ORDER BY DC.Id_DC)) AS Nro,
		CONVERT(INT, ROW_NUMBER() OVER(ORDER BY DC.Id_DC)) AS Nro,
		--rank() OVER (ORDER BY DC.Id_DC) AS NRO,
		CTP.Code AS 'CodigoDocumento',
		[CTP].[Desc] AS Descripcion,
		CONVERT(VARCHAR(4),SUBSTRING(DC.ID_CPE,1,4)) AS Serie,
		DC.NUM_CPE AS NumeroDocumento,
		DC.NUM_DOCUMENTO AS NumeroFactura,
		DC.FEC_EMIS AS FechaEmision,
		DC.RE_NUMDOC AS NumDocCliente,
		DC.RE_NOMBRE AS Cliente,
		DC.RE_DIRECCION AS ClienteDireccion,
		DC.TOT_IMPOR_TOTAL AS MontoTotal,
		[FED].[Desc] AS Estado,
		[FED].[RutaImagen] AS RutaImagen
		,CASE DC.TPO_MONEDA 
		WHEN 'PEN' THEN 'SOLES'
		WHEN 'USD' THEN 'DÓLARES AMERICANOS' 
		END AS Moneda,
		DC.EM_NUM_DOCU AS EmpresaRuc,
		DC.EM_NOMBRE AS RazonSocial,
		DC.EM_DFISCAL AS EmpresaDireccion,
		DC.EM_UBIGEO AS EmpresaUbigeo,
		MD.Descripcion AS EmpresaUbigeoDesc,
		CTP.Id_TD AS IdTipoDocumento,
		ISNULL(DC.VAR_FIR, 0) AS 'XML',
		DC.[CAMPO1] AS Campo1
	FROM [Fact].[O.DocumentoCabecera] AS DC LEFT JOIN
		 [Ctl].[TipoDocumento] AS CTP ON DC.ID_TPO_CPE = CTP.Code LEFT JOIN
		 [Fact].[O.EstadoDocumento] AS FED ON DC.Id_ED = FED.Id_ED LEFT JOIN 
		 [Mtro].[Distrito] AS MD ON DC.EM_UBIGEO = MD.CodigoUbigeo
	WHERE (@TipoDocumento = 0 OR CTP.Id_TD = @TipoDocumento) AND
		  (DC.EM_NUM_DOCU = @RucEmpresa) AND
		  (@ClienteRuc_Dni = '' OR DC.RE_NUMDOC = @ClienteRuc_Dni) AND
		  ((@FechaInicio = '' AND @FechaFin = '') OR (DC.[FEC_EMIS] BETWEEN @FechaInicio AND @FechaFin))
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_ListaHistorialCliente_EmpresaPort]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_ListaHistorialCliente_EmpresaPort]
	@TipoDocumento [int],
	@RucEmpresa [varchar](11),
	@ClienteRuc_Dni [varchar](50),
	@FechaInicio [varchar](10),
	@FechaFin [varchar](10)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		--CONVERT(INT,RANK() OVER (ORDER BY DC.Id_DC)) AS Nro,
		CONVERT(INT, ROW_NUMBER() OVER(ORDER BY DC.Id_DC)) AS Nro,
		--rank() OVER (ORDER BY DC.Id_DC) AS NRO,
		CTP.Code AS 'CodigoDocumento',
		[CTP].[Desc] AS Descripcion,
		CONVERT(VARCHAR(4),SUBSTRING(DC.ID_CPE,1,4)) AS Serie,
		DC.NUM_CPE AS NumeroDocumento,
		DC.NUM_DOCUMENTO AS NumeroFactura,
		DC.FEC_EMIS AS FechaEmision,
		DC.RE_NUMDOC AS NumDocCliente,
		DC.RE_NOMBRE AS Cliente,
		DC.RE_DIRECCION AS ClienteDireccion,
		DC.TOT_IMPOR_TOTAL AS MontoTotal,
		[FED].[Desc] AS Estado,
		[FED].[RutaImagen] AS RutaImagen
		,CASE DC.TPO_MONEDA 
		WHEN 'PEN' THEN 'Soles'
		WHEN 'USD' THEN 'Dólares' 
		END AS Moneda,
		DC.EM_NUM_DOCU AS EmpresaRuc,
		DC.EM_NOMBRE AS RazonSocial,
		DC.EM_DFISCAL AS EmpresaDireccion,
		'' AS EmpresaUbigeo,
		'' AS EmpresaUbigeoDesc,
		--DC.EM_UBIGEO AS EmpresaUbigeo,
		--MD.Descripcion AS EmpresaUbigeoDesc,
		CTP.Id_TD AS IdTipoDocumento,
		--DC.[XML_SIGN] AS 'XML'

		--DC.[VAR_FIR] AS 'XML'
		ISNULL(DC.[VAR_FIR], 0) AS 'XML',
		DC.[CAMPO1] AS Campo1,
		CONVERT(INT,DC.[CAMPO2]) AS TypeFormat
	FROM [Fact].[O.DocumentoCabecera] AS DC LEFT JOIN
		 [Ctl].[TipoDocumento] AS CTP ON DC.ID_TPO_CPE = CTP.Code LEFT JOIN
		 [Fact].[O.EstadoDocumento] AS FED ON DC.Id_ED = FED.Id_ED --LEFT JOIN 
		 --[Mtro].[Distrito] AS MD ON DC.EM_UBIGEO = MD.CodigoUbigeo
	WHERE (@TipoDocumento = 0 OR CTP.Id_TD = @TipoDocumento) AND
		  (DC.EM_NUM_DOCU = @RucEmpresa) AND
		  (@ClienteRuc_Dni = '' OR DC.RE_NUMDOC = @ClienteRuc_Dni) AND
		  ((@FechaInicio = '' AND @FechaFin = '') OR (DC.[FEC_EMIS] BETWEEN @FechaInicio AND @FechaFin))
END
GO
/****** Object:  StoredProcedure [Fact].[Usp_ObtieneCorrelativo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_ObtieneCorrelativo] --'2016-10-05', 'RA', '20431084172'
	@Fecha AS VARCHAR(10),
   @Tipo  AS VARCHAR(2),
   @Ruc   AS VARCHAR(11)
AS
   SET NOCOUNT ON;
   IF @Tipo = 'RA'
      BEGIN
         SELECT 
            --CONVERT(INT, MAX(SUBSTRING(NUM_CPE,25,3))) + 1 as 'ID_RAC'
            COUNT(ID_RAC) + 1
         FROM 
            [Fact].[O.RBajasCabecera]
         WHERE
            FEC_ENV = CONVERT(DATETIME,@Fecha,20) AND
            TIPO = 'RA' AND
            RucEntity = @Ruc;
      END
   ELSE IF @Tipo = 'RR'
      BEGIN
         SELECT 
            --MAX(SUBSTRING(NUM_CPE,25,3)) + 1
            COUNT(ID_RAC) + 1
         FROM 
            [Fact].[O.RBajasCabecera]
         WHERE
            FEC_ENV = CONVERT(DATETIME,@Fecha,20) AND
            TIPO = 'RR' AND
            RucEntity = @Ruc;
      END
   ELSE IF @Tipo = 'RC'
      BEGIN
         SELECT 
            --MAX(SUBSTRING(NUM_CPE,25,3)) + 1
            COUNT(ID_RBC) + 1
         FROM 
            [Fact].[O.RBoletasCabecera]
         WHERE
            FEC_ENV = CONVERT(DATETIME,@Fecha,20) AND
            RucEntity = @Ruc;
      END

GO
/****** Object:  StoredProcedure [Fact].[Usp_ObtieneRegex]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Fact].[Usp_ObtieneRegex]

AS
BEGIN
    SET NOCOUNT ON;
    SELECT (TAB + '-' + POS) AS 'KEY', NOM, VAL, MND, DOC, TAB, MSG
	FROM [Fact].[CONFIG_MAIN]
END


GO
/****** Object:  StoredProcedure [Fact].[Usp_ObtieneResumenA]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_ObtieneResumenA] --'2016-12-08','RA','20216414056'
	@Fecha AS VARCHAR(10),
	@Type  AS VARCHAR(2),
	@Ruc   AS VARCHAR(11)
AS

   SET NOCOUNT ON;
   IF @Type = 'RA'
      BEGIN
         SELECT
            D.Code,
            A.Serie,
            A.Correlativo,
            A.Motivo_Anul
         FROM 
            [Fact].[O.Documento_Anul] AS A 
            INNER JOIN [Fact].[O.DocumentoCabecera] AS C ON A.NUM_CE = C.NUM_CPE
            INNER JOIN [Ctl].[TipoDocumento] AS D ON A.Id_TpoDoc = D.Id_TD
         WHERE
            A.Fecha_Anul = CONVERT(DATETIME,@Fecha,20) AND
            A.RucEmpresa = @Ruc AND
            A.Serie NOT LIKE 'R%' AND
            C.VOIDED IS NULL
         ORDER BY A.TypeDoc, A.Serie, A.Correlativo ;
      END
   ELSE IF @Type = 'RR'
      BEGIN
         --SELECT
         --   D.Code,
         --   A.Serie,
         --   A.Correlativo,
         --   A.Motivo_Anul
         --FROM 
         --   [Fact].[O.Documento_Anul] AS A,
         --   [Fact].[O.DocumentoCabecera] AS C,
         --   [Ctl].[TipoDocumento] AS D
         --WHERE
         --   A.Fecha_Anul = CONVERT(DATETIME,@Fecha,20) AND
         --   A.Serie = C.SERIE AND
         --   A.Correlativo = C.NUM_DOCUMENTO AND
         --   A.Id_TpoDoc = D.Id_TD AND
         --   C.SERIE LIKE 'R%' AND
         --   C.EM_NUM_DOCU = @Ruc;
         SELECT
            D.Code,
            A.Serie,
            A.Correlativo,
            A.Motivo_Anul
         FROM 
            [Fact].[O.Documento_Anul] AS A 
            INNER JOIN [Fact].[O.DocumentoCabecera] AS C ON A.NUM_CE = C.NUM_CPE
            INNER JOIN [Ctl].[TipoDocumento] AS D ON A.Id_TpoDoc = D.Id_TD
         WHERE
            A.Fecha_Anul = CONVERT(DATETIME,@Fecha,20) AND
            A.RucEmpresa = @Ruc AND
            A.Serie LIKE 'R%' AND
            C.VOIDED IS NULL
         ORDER BY A.TypeDoc, A.Serie, A.Correlativo ;
      END

GO
/****** Object:  StoredProcedure [Fact].[Usp_ObtieneResumenB]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_ObtieneResumenB] --'2017-03-13','20216414056'
  @Fecha AS VARCHAR(10),
  @Ruc   AS VARCHAR(11)
AS
BEGIN
  DECLARE @CC DECIMAL(10,5) = ( SELECT cambio FROM Fact.ExchangeRate WHERE RucNumber = @Ruc AND Fecha = CONVERT(DATETIME,@Fecha,20));
  --DECLARE @FechaRes VARCHAR(10) = @Fecha
  --DECLARE @CC DECIMAL(10,5) = ( SELECT cambio FROM Fact.ExchangeRate WHERE Fecha BETWEEN @Fecha AND @Fecha);
  SET NOCOUNT ON;
  SELECT ID_TPO_CPE AS TPODOC,
      SERIE, 
      CAST(NUM_DOCUMENTO AS INT) AS NUMERO, 
      FEC_EMIS AS DESDE, 
      FEC_EMIS AS HASTA,
       
       -- MONTOS 
      TOT_GRAV_MTO AS GRAVADO, 
      TOT_EXON_MTO AS EXONERADO, 
      TOT_INAF_MTO AS INAFECTO,
      TOT_SUM_OCARG AS OCARGOS,

       -- IMPUESTOS
      TOT_SUMA_IGV AS IGV, 
      TOT_SUMA_ISC AS ISC,
      TOT_SUMA_OTRIB AS OTROS,
       
       -- TOTAL
      TOT_IMPOR_TOTAL AS TOTAL,
    
       -- TIPO DE CAMBIO
      TPO_MONEDA AS MONEDA,
      ISNULL(@CC,0.0) AS CAMBIO
       --(SELECT cambio FROM Fact.ExchangeRate WHERE RucNumber = @Ruc AND Fecha = CONVERT(DATETIME,@Fecha,20)) AS CAMBIO
  FROM [Fact].[O.DocumentoCabecera]
  --WHERE (FEC_EMIS BETWEEN @FechaRes AND @FechaRes) --AND 
  WHERE FEC_EMIS = CONVERT(DATETIME,@Fecha,20) AND 
         (ID_TPO_CPE = '03' OR SERIE LIKE 'B%') AND
         EM_NUM_DOCU = @Ruc AND VOIDED IS NULL
  ORDER BY NUM_CPE ;
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_ObtieneResumenB_Ant]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_ObtieneResumenB_Ant] --'2017-03-13','20216414056'
  @Fecha AS VARCHAR(10),
  @Ruc   AS VARCHAR(11)
AS
BEGIN
  DECLARE @CC DECIMAL(10,5) = ( SELECT cambio FROM Fact.ExchangeRate WHERE RucNumber = @Ruc AND Fecha = CONVERT(DATETIME,@Fecha,20));
  --DECLARE @FechaRes VARCHAR(10) = @Fecha
  --DECLARE @CC DECIMAL(10,5) = ( SELECT cambio FROM Fact.ExchangeRate WHERE Fecha BETWEEN @Fecha AND @Fecha);
  SET NOCOUNT ON;
  SELECT ID_TPO_CPE AS TPODOC,
      SERIE, 
      CAST(NUM_DOCUMENTO AS INT) AS NUMERO, 
      FEC_EMIS AS DESDE, 
      FEC_EMIS AS HASTA,
       
       -- MONTOS 
      TOT_GRAV_MTO AS GRAVADO, 
      TOT_EXON_MTO AS EXONERADO, 
      TOT_INAF_MTO AS INAFECTO,
      TOT_SUM_OCARG AS OCARGOS,

       -- IMPUESTOS
      TOT_SUMA_IGV AS IGV, 
      TOT_SUMA_ISC AS ISC,
      TOT_SUMA_OTRIB AS OTROS,
       
       -- TOTAL
      TOT_IMPOR_TOTAL AS TOTAL,
    
       -- TIPO DE CAMBIO
      TPO_MONEDA AS MONEDA,
      ISNULL(@CC,0.0) AS CAMBIO
       --(SELECT cambio FROM Fact.ExchangeRate WHERE RucNumber = @Ruc AND Fecha = CONVERT(DATETIME,@Fecha,20)) AS CAMBIO
  FROM [Fact].[O.DocumentoCabecera]
  --WHERE (FEC_EMIS BETWEEN @FechaRes AND @FechaRes) --AND 
  WHERE FEC_EMIS = CONVERT(DATETIME,@Fecha,20) AND 
         (ID_TPO_CPE = '03' OR SERIE LIKE 'B%') AND
         EM_NUM_DOCU = @Ruc AND VOIDED IS NULL AND 
         SUMMARY IS NULL
  ORDER BY NUM_CPE ;
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_ObtieneTipoDocumento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_ObtieneTipoDocumento]
	@Codigo [varchar](10)
WITH EXECUTE AS CALLER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdEstado
	FROM [Ctl].[TipoDocumento]
	WHERE Code = @Codigo
END
GO
/****** Object:  StoredProcedure [Fact].[Usp_ResumenBajasCampoXML]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_ResumenBajasCampoXML]
	@IdRAC [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		[VAR_FIR]
	FROM [Fact].[O.RBajasCabecera] AS FBC
	WHERE FBC.[ID_RAC] = @IdRAC
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_UpdateStatusDocPrint]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_UpdateStatusDocPrint]
@ID_DC INT,
@NUM_CPE VARCHAR(40)
AS
BEGIN
	UPDATE [Fact].[O.DocumentoCabecera]
		SET [StatusPrint] = 2
	WHERE [ID_DC] = @ID_DC AND
		  [NUM_CPE] = @NUM_CPE
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_UpdateStatusDocSend]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_UpdateStatusDocSend]
@ID_DC INT,
@NUM_CPE VARCHAR(40)
AS
BEGIN
	UPDATE [Fact].[O.DocumentoCabecera]
		SET [StatuSend] = 2
	WHERE [ID_DC] = @ID_DC AND
		  [NUM_CPE] = @NUM_CPE
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_ValidarDocumentoExiste]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_ValidarDocumentoExiste]
	@TipoDocumento [varchar](2),
	@NumeroSerie [varchar](10),
	@NumeroDocumento [varchar](15),
	@RucEmpresa [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		FDC.[FEC_EMIS] AS FechaEmision,
		FDC.[TOT_IMPOR_TOTAL] AS MontoTotal
	FROM [Fact].[O.DocumentoCabecera] AS FDC
	WHERE [ID_TPO_CPE] = @TipoDocumento AND
		  [SERIE] = @NumeroSerie AND
		  [NUM_DOCUMENTO] = @NumeroDocumento AND
		  FDC.[EM_NUM_DOCU] = @RucEmpresa
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_ValidarExistsDocumento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Fact].[Usp_ValidarExistsDocumento]
	@NUM_CPE [varchar](30)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		[NUM_CPE]
	FROM [Fact].[O.DocumentoCabecera]
	WHERE [NUM_CPE] = @NUM_CPE
END


GO
/****** Object:  StoredProcedure [Fact].[Usp_VerificarStatusDocxPrint_Parameter]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [Fact].[Usp_VerificarStatusDocxPrint_Parameter] --'20431084172-20-R001-00000062'
@NUM_CE VARCHAR(40)
AS
BEGIN
	SELECT  
		DC.[Id_DC] AS Id,
		DC.[NUM_CPE] AS NombreArchivo,
		DC.[ID_TPO_CPE] AS TipoDocumento,
		DC.[ID_CPE] AS SerieCorrelativo,
		DC.[Id_ED] AS IdEstado,
		DE.[Desc] AS DescEstado,
		DC.[VAR_FIR] AS 'XML',
		DC.[EM_NUM_DOCU] AS RucEmpresa,
		--DC.[RE_NUMDOC] AS 'ClienteRuc',
		--ISNULL(FME.[PARA], 'SinDestino') AS Para,
		--ISNULL(FME.[CC], 'SinCC') AS CC,
		--ISNULL(FME.[CCO], 'SinCCO') AS CCO,
		DC.[CAMPO1] AS Campo1,
		--DC.[REF_FILES],
		DC.[IMPRESORA] AS Impresora,
		CONVERT(INT,DC.[CAMPO2]) AS TypeFormat
	FROM [Fact].[O.DocumentoCabecera] AS DC LEFT JOIN
		 [Fact].[O.FlexMailEnvio] AS FME ON DC.[ID_DC] = FME.[Id_DC] LEFT JOIN
		 [Fact].[O.EstadoDocumento] AS DE ON DC.Id_ED = DE.Id_ED
	WHERE (DC.[NUM_CPE] = @NUM_CE) 
		  AND [StatusPrint] IS NULL
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_VerificarStatusDocxSend]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_VerificarStatusDocxSend]
AS
BEGIN
	SELECT  
		DC.[Id_DC] AS Id,
		DC.[NUM_CPE] AS NombreArchivo,
		DC.[ID_TPO_CPE] AS TipoDocumento,
		DC.[ID_CPE] AS SerieCorrelativo,
		DC.[Id_ED] AS IdEstado,
		DE.[Desc] AS DescEstado,
		DC.[VAR_FIR] AS 'XML',
		DC.[EM_NUM_DOCU] AS RucEmpresa,
		DC.[RE_NUMDOC] AS 'ClienteRuc',
		ISNULL(FME.[PARA], 'SinDestino') AS Para,
		ISNULL(FME.[CC], 'SinCC') AS CC,
		ISNULL(FME.[CCO], 'SinCCO') AS CCO,
		DC.[CAMPO1] AS Campo1,
		DC.[REF_FILES],
		CONVERT(INT,DC.[CAMPO2]) AS TypeFormat
	FROM [Fact].[O.DocumentoCabecera] AS DC LEFT JOIN
		 [Fact].[O.FlexMailEnvio] AS FME ON DC.[ID_DC] = FME.[Id_DC] LEFT JOIN
		 [Fact].[O.EstadoDocumento] AS DE ON DC.Id_ED = DE.Id_ED
	WHERE ([ID_TPO_CPE] = '20' OR [ID_TPO_CPE] = '40') AND DE.[Id_ED] = 7
		  AND [StatuSend] IS NULL
	UNION
	SELECT  
		DC.[Id_DC] AS Id,
		DC.[NUM_CPE] AS NombreArchivo,
		DC.[ID_TPO_CPE] AS TipoDocumento,
		DC.[ID_CPE] AS SerieCorrelativo,
		DC.[Id_ED] AS IdEstado,
		DE.[Desc] AS DescEstado,
		DC.[VAR_FIR] AS 'XML',
		DC.[EM_NUM_DOCU] AS RucEmpresa,
		DC.[RE_NUMDOC] AS 'ClienteRuc',
		ISNULL(FME.[PARA], 'SinDestino') AS Para,
		ISNULL(FME.[CC], 'SinCC') AS CC,
		ISNULL(FME.[CCO], 'SinCCO') AS CCO,
		DC.[CAMPO1] AS Campo1,
		DC.[REF_FILES],
		CONVERT(INT,DC.[CAMPO2]) AS TypeFormat
	FROM [Fact].[O.DocumentoCabecera] AS DC LEFT JOIN
		 [Fact].[O.FlexMailEnvio] AS FME ON DC.[NUM_CPE] = FME.[NUM_CPE] LEFT JOIN
		 [Fact].[O.EstadoDocumento] AS DE ON DC.Id_ED = DE.Id_ED
	--WHERE ([ID_TPO_CPE] = '01' OR [ID_TPO_CPE] = '03' OR [ID_TPO_CPE] = '07' OR [ID_TPO_CPE] = '08' ) 
	WHERE ([ID_TPO_CPE] = '01' OR [ID_TPO_CPE] = '07' OR [ID_TPO_CPE] = '08' ) 
		  AND DE.[Id_ED] = 7
		  AND [StatuSend] IS NULL
	UNION
	SELECT  
		DC.[Id_DC] AS Id,
		DC.[NUM_CPE] AS NombreArchivo,
		DC.[ID_TPO_CPE] AS TipoDocumento,
		DC.[ID_CPE] AS SerieCorrelativo,
		DC.[Id_ED] AS IdEstado,
		DE.[Desc] AS DescEstado,
		DC.[VAR_FIR] AS 'XML',
		DC.[EM_NUM_DOCU] AS RucEmpresa,
		DC.[RE_NUMDOC] AS 'ClienteRuc',
		ISNULL(FME.[PARA], 'SinDestino') AS Para,
		ISNULL(FME.[CC], 'SinCC') AS CC,
		ISNULL(FME.[CCO], 'SinCCO') AS CCO,
		DC.[CAMPO1] AS Campo1,
		DC.[REF_FILES],
		CONVERT(INT,DC.[CAMPO2]) AS TypeFormat
	FROM [Fact].[O.DocumentoCabecera] AS DC LEFT JOIN
		 [Fact].[O.FlexMailEnvio] AS FME ON DC.[ID_DC] = FME.[Id_DC] LEFT JOIN
		 [Fact].[O.EstadoDocumento] AS DE ON DC.Id_ED = DE.Id_ED
	WHERE ([ID_TPO_CPE] = '03') AND DE.[Id_ED] = 2
		  AND [StatuSend] IS NULL
END

GO
/****** Object:  StoredProcedure [Fact].[Usp_VerificarStatusDocxSend_Parameter]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Fact].[Usp_VerificarStatusDocxSend_Parameter] --'20431084172-20-R001-00000062'
@NUM_CE VARCHAR(40)
AS
BEGIN
	SELECT  
		DC.[Id_DC] AS Id,
		DC.[NUM_CPE] AS NombreArchivo,
		DC.[ID_TPO_CPE] AS TipoDocumento,
		DC.[ID_CPE] AS SerieCorrelativo,
		DC.[Id_ED] AS IdEstado,
		DE.[Desc] AS DescEstado,
		DC.[VAR_FIR] AS 'XML',
		DC.[EM_NUM_DOCU] AS RucEmpresa,
		DC.[RE_NUMDOC] AS 'ClienteRuc',
		ISNULL(FME.[PARA], 'SinDestino') AS Para,
		ISNULL(FME.[CC], 'SinCC') AS CC,
		ISNULL(FME.[CCO], 'SinCCO') AS CCO,
		DC.[CAMPO1] AS Campo1,
		DC.[REF_FILES],
		CONVERT(INT,DC.[CAMPO2]) AS TypeFormat
	FROM [Fact].[O.DocumentoCabecera] AS DC LEFT JOIN
		 [Fact].[O.FlexMailEnvio] AS FME ON DC.[ID_DC] = FME.[Id_DC] LEFT JOIN
		 [Fact].[O.EstadoDocumento] AS DE ON DC.Id_ED = DE.Id_ED
	WHERE (DC.[NUM_CPE] = @NUM_CE) 
		  AND [StatuSend] IS NULL
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ActualizarCliente]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ActualizarCliente]
	@IdCliente [int],
	@RazonSocial [varchar](250),
	@NroDocumento [varchar](11),
	@Email [varchar](250),
	@Telefono [varchar](50),
	@Direccion [varchar](250),
	@IdEstado [int],
	@IdEmpresa [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	UPDATE [Mtro].[Cliente] SET
		RazonSocial = @RazonSocial,
		--NroDocumento = @NroDocumento,
		Email = @Email,
		Telefono = @Telefono,
		Direccion = @Direccion,
		IdEstado = @IdEstado,
		IdEmpresa = @IdEmpresa
	WHERE IdCliente = @IdCliente AND
		  NroDocumento = @NroDocumento
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ActualizarEmpresa]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ActualizarEmpresa]
	@IdEmpresa [int],
	@CodEmpresa [varchar](20),
	@Ubigeo [int],
	@CodigoUbigeo [varchar](6),
	@RUC [varchar](11),
	@RazonSocial [varchar](50),
	@RazonComercial [varchar](50),
	@Telefono [varchar](15),
	@Fax [varchar](15),
	@Direccion [varchar](100),
	@DomicilioFiscal [varchar](120),
	@Urbanizacion [varchar](200),
	@FechaRegistro [datetime],
	@PaginaWeb [varchar](150),
	@Estado [int],
	@Email [varchar](120),
	@Id_TDI [int],
    @Url_CompanyLogo [varchar](max),
	@Url_CompanyConsult [varchar](max)
WITH EXECUTE AS CALLER
AS
BEGIN
	UPDATE [Mtro].[Empresa]
		SET
			CodEmpresa=@CodEmpresa
			,IdUbigeo=@Ubigeo
			,Ubigeo=@CodigoUbigeo
			,Ruc=@RUC
			,RazonSocial=@RazonSocial
			,RazonComercial=@RazonComercial
			,Telefono=@Telefono
			,Fax=@Fax
			,Direccion=@Direccion
			,DomicilioFiscal=@DomicilioFiscal
			,Urbanizacion = @Urbanizacion
			,PaginaWeb=@PaginaWeb
			,IdEstado=@Estado
			,Email=@Email
			,Id_TDI=@Id_TDI
			,Url_CompanyLogo = @Url_CompanyLogo
			,Url_CompanyConsult = @Url_CompanyConsult
	WHERE IdEmpresa = @IdEmpresa
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_GetIdProvinciaForDistrito]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Mtro].[Usp_GetIdProvinciaForDistrito] --'010101'
@CODUBIGEO VARCHAR(6)
AS
BEGIN
	--DECLARE @CODUBIGEO VARCHAR(6) = '150501'
	DECLARE @CODDEP VARCHAR(2) = SUBSTRING(@CODUBIGEO, 1, 2)
	DECLARE @CODPROV VARCHAR(2) = SUBSTRING(@CODUBIGEO, 3, 2)
	DECLARE @CODDIST VARCHAR(2) = SUBSTRING(@CODUBIGEO, 5, 2)
	DECLARE @IdDepart INT = (SELECT Id FROM [Mtro].[Departamento] WHERE Codigo = @CODDEP)
	DECLARE @IdProvincia INT = (SELECT Id FROM [Mtro].[Provincia] WHERE Codigo = @CODPROV AND CodDepartamento = @CODDEP)
	DECLARE @IdDistrito INT = (SELECT Id FROM [Mtro].[Distrito] WHERE CodigoUbigeo = @CODDEP + @CODPROV + @CODDIST)
	UPDATE [Mtro].[Distrito] SET IdProvincia = @IdProvincia WHERE CodigoUbigeo = @CODUBIGEO
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_GetListCtaBank]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Mtro].[Usp_GetListCtaBank]
@RucEntity VARCHAR(11)
AS
BEGIN
	SELECT 
		Id,
		Code,
		BankName,
		CtaSoles,
		CtaDolares,
		RucEntity AS RUC,
		TypeBank,
		ME.IdEstado,
		ME.Descripcion
	FROM [Mtro].[Estado] AS ME INNER JOIN 
		 [Mtro].[BankCta] AS MB ON ME.IdEstado = MB.IdEstado
	WHERE RucEntity = @RucEntity
END


GO
/****** Object:  StoredProcedure [Mtro].[Usp_Insert_CtaBank]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Mtro].[Usp_Insert_CtaBank]
@Code VARCHAR(30),
@BankName VARCHAR(250),
@CtaSoles VARCHAR(50),
@CtaDolares VARCHAR(50),
@RucEntity VARCHAR(11),
@TypeBank INT,
@IdEstado INT
AS
BEGIN
	INSERT INTO [Mtro].[BankCta] 
		(Code, BankName, CtaSoles, CtaDolares, RucEntity, TypeBank, IdEstado)
		VALUES
		(@Code, @BankName, @CtaSoles, @CtaDolares, @RucEntity, @TypeBank, @IdEstado)
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_InsertarCliente]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_InsertarCliente]
	@RazonSocial [varchar](250),
	@NroDocumento [varchar](11),
	@Email [varchar](250),
	@Telefono [varchar](50),
	@Direccion [varchar](250),
	@IdEstado [int],
	@IdEmpresa [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Mtro].[Cliente]
		(RazonSocial, NroDocumento, Email, Telefono, Direccion, IdEstado, IdEmpresa)
		VALUES
		(@RazonSocial, @NroDocumento, @Email, @Telefono, @Direccion, @IdEstado, @IdEmpresa)
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_InsertarEmpresa]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_InsertarEmpresa]
	@CodEmpresa [varchar](20),
	@IdUbigeo [int],
	@Ubigeo [varchar](6),
	@Ruc [varchar](11),
	@RazonSocial [varchar](50),
	@RazonComercial [varchar](50),
	@Telefono [varchar](15),
	@Fax [varchar](15),
	@Direccion [varchar](120),
	@DomicilioFiscal [varchar](120),
	@Urbanizacion [varchar](150),
	@FechaRegistro [datetime],
	@PaginaWeb [varchar](150),
	@Email [varchar](150),
	@IdEstado [int],
	@Id_TDI [int],
	@TpoLogin [varchar](20),
	@Url_CompanyLogo [varchar](max),
	@Url_CompanyConsult [varchar](max)
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Mtro].[Empresa] (
				CodEmpresa, IdUbigeo, Ubigeo
				,Ruc, RazonSocial, RazonComercial
				,Telefono,Fax
				,Direccion, DomicilioFiscal
				,Urbanizacion, FechaRegistro
				,PaginaWeb, Email
				,IdEstado, Id_TDI, TpoLogin, Url_CompanyLogo, Url_CompanyConsult)
				output inserted.IdEmpresa
				VALUES (
				@CodEmpresa, @IdUbigeo, @Ubigeo
				,@Ruc, @RazonSocial, @RazonComercial
				,@Telefono, @Fax, @Direccion
				,@DomicilioFiscal, @Urbanizacion
				,@FechaRegistro, @PaginaWeb
				,@Email, @IdEstado, @Id_TDI, @TpoLogin, @Url_CompanyLogo, @Url_CompanyConsult)
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_InsertDepartamento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Mtro].[Usp_InsertDepartamento]
@CodDepart VARCHAR(2),
@Descripcion VARCHAR(150)
AS
IF NOT EXISTS (SELECT Codigo FROM [Mtro].[Departamento] WHERE Codigo = @CodDepart)
BEGIN
	INSERT INTO [Mtro].[Departamento]
		(Codigo, Descripcion, IdPais, CodPais)
	VALUES
		(@CodDepart, @Descripcion, 1, 'PE')
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_InsertDistritoxProvincia]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Mtro].[Usp_InsertDistritoxProvincia]
@CodDistrito VARCHAR(02),
@Descripcion VARCHAR(150),
@CodProvincia VARCHAR(2),
@CodDepartamento VARCHAR(2)
AS
--IF NOT EXISTS (SELECT Id FROM [Mtro].[Distrito] WHERE [CodigoUbigeo] = @CodDepartamento + @CodProvincia + @CodDistrito)
BEGIN
	INSERT INTO [Mtro].[Distrito]
			([Codigo],
			[Descripcion],
			--[IdProvincia],
			[CodProvincia],
			[CodigoUbigeo])
		VALUES
			(@CodDistrito, @Descripcion, @CodProvincia,
			(@CodDepartamento + @CodProvincia + @CodDistrito))
END
GO
/****** Object:  StoredProcedure [Mtro].[Usp_InsertProvinciaxDepartmento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Mtro].[Usp_InsertProvinciaxDepartmento]
@CodProvincia VARCHAR(02),
@Descripcion VARCHAR(150),
@CodDepartamament VARCHAR(2)
AS
IF NOT EXISTS (SELECT Id FROM [Mtro].[Provincia] WHERE Descripcion = @Descripcion)
BEGIN
	INSERT INTO [Mtro].[Provincia]
			([Codigo],
			[Descripcion],
			[IdDepartamento],
			[CodDepartamento])
		VALUES
			(@CodProvincia, @Descripcion, 
			(SELECT [Id] FROM [Mtro].[Departamento] WHERE Codigo = @CodDepartamament), @CodDepartamament)
END
GO
/****** Object:  StoredProcedure [Mtro].[Usp_ListaCliente]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ListaCliente]
	@RazonSocial [varchar](250),
	@NroDocumento [varchar](11),
	@IdEstado [int],
	@IdEmpresa [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		MC.IdCliente,
		MC.RazonSocial,
		MC.NroDocumento AS NroDocumento,
		MC.Email,
		MC.Telefono,
		MC.Direccion,
		MC.IdEstado,
		ME.Descripcion,
		MEE.IdEmpresa,
		MEE.Ruc AS RUC,
		MEE.RazonSocial AS RazonSocial
	FROM [Mtro].[Cliente] AS MC INNER JOIN
		 [Mtro].[Estado] AS ME ON MC.IdEstado = ME.IdEstado INNER JOIN
		 [Mtro].[Empresa] AS MEE ON MC.IdEmpresa = MEE.IdEmpresa
	WHERE (@RazonSocial = '' OR MC.RazonSocial LIKE @RazonSocial + '%') AND
		  (@NroDocumento = '' OR MC.NroDocumento LIKE @NroDocumento + '%') AND
		  (@IdEstado = 0 OR ME.IdEstado = @IdEstado) AND
		  (MEE.IdEmpresa = @IdEmpresa)
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ListaDepartamento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ListaDepartamento]
	@IdPais [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		[Id]
		,[Codigo]
		,[Descripcion]
	FROM [Mtro].[Departamento]
	WHERE [IdPais] = @IdPais
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ListaDistrito]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ListaDistrito]
	@IdProvincia [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT			
		[Id]
		,RIGHT([CodigoUbigeo],2) AS Codigo
		,[CodigoUbigeo]
		,[Descripcion]
	FROM [Mtro].[Distrito]
	WHERE [IdProvincia] = @IdProvincia
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ListadoEmpresa]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ListadoEmpresa] --0,'20101071562','','','',0
	@Ubigeo [int],
	@RUC [varchar](11),
	@RazonSocial [varchar](50),
	@RazonComercial [varchar](50),
	@Telefono [varchar](15),
	@Estado [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		CONVERT(INT,RANK() OVER (ORDER BY ME.CodEmpresa)) AS Nro
		,ME.IdEmpresa
		,ME.CodEmpresa
		--,ME.IdUbigeo
		,CONVERT(VARCHAR(6),ME.Ubigeo) AS Ubigeo
		,MD.Descripcion AS DescripcionUbigeo
		,ME.Ruc
		,ME.RazonSocial
		,ME.RazonComercial
		,ME.Direccion
		,ME.DomicilioFiscal
		,ME.Urbanizacion
		,ME.Telefono
		,ME.Fax
		,ME.PaginaWeb
		,ME.Email
		,ME.FechaRegistro
		,MET.IdEstado
		,MET.Descripcion AS DescripcionEstado
		,CTD.Id_TDI AS IdTipoDocumento
		,CTD.[Desc] AS DescripcionTipoDocumento,
		ISNULL(ME.Url_CompanyLogo, '') AS Url_CompanyLogo
		,ISNULL(ME.Url_CompanyConsult, '') AS Url_CompanyConsult
	FROM [Mtro].[Empresa] AS ME LEFT JOIN
		--[Mtro].[Ubigeo] AS MU ON ME.IdUbigeo = MU.IdUbigeo INNER JOIN
		[Mtro].[Estado] AS MET ON ME.IdEstado = MET.IdEstado LEFT JOIN
		[Ctl].[TipoDocumentoIdentidad] AS CTD ON ME.Id_TDI = CTD.Id_TDI LEFT JOIN
		[Mtro].[Distrito] AS MD ON ME.Ubigeo = MD.CodigoUbigeo
	WHERE 
		--(@CodEmpresa = '' OR ME.CodEmpresa LIKE '%' + @CodEmpresa + '%') AND
		--(@RUC = '' OR ME.Ruc LIKE @RUC + '%') AND
		(ME.Ruc = @RUC ) AND
		(@RazonSocial = '' OR ME.RazonSocial LIKE @RazonSocial + '%') AND
		(@RazonComercial = '' OR ME.RazonComercial LIKE @RazonComercial + '%') AND
		--(@Telefono = '' OR ME.Telefono LIKE @Telefono + '%') AND
		(@Estado = 0 OR MET.IdEstado = @Estado) --AND
		--(@Ubigeo = 0 OR MU.IdUbigeo = @Ubigeo)
	--ORDER BY ME.RazonSocial
END


--SELECT COUNT(id) AS TOTAL FROM [Mtro].[Provincia]

--SELECT ID FROM [Mtro].[Distrito] WHERE CodigoUbigeo = '150131'

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ListaEmpresa]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ListaEmpresa]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		IdEmpresa
		,CodEmpresa
		,RazonSocial
		,ISNULL([TpoLogin], '-') AS [TpoLogin]
	FROM [Mtro].[Empresa]
END


GO
/****** Object:  StoredProcedure [Mtro].[Usp_ListaEstado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ListaEstado]
WITH EXECUTE AS CALLER
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		IdEstado AS IdEstado
		,Descripcion AS Descripcion
	FROM [Mtro].[Estado]
END


GO
/****** Object:  StoredProcedure [Mtro].[Usp_ListaPais]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ListaPais]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		[Id]
		,[Codigo]
		,[Descripcion]
	FROM [Mtro].[Pais]
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ListaProvincia]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ListaProvincia]
	@IdDepartamento [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		[Id]
		,[Codigo]
		,[Descripcion]
	FROM [Mtro].[Provincia]
	WHERE [IdDepartamento] = @IdDepartamento
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ObtenerCodPais]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ObtenerCodPais]
	@CodUbigeo [varchar](6)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		 MPS.[Cod_Pais]
	FROM [Mtro].[Pais] AS MPS INNER JOIN
		 [Mtro].[Departamento] AS MD ON MPS.Id = MD.IdPais INNER JOIN
		 [Mtro].[Provincia] AS MP ON MD.Id = MP.IdDepartamento INNER JOIN
		 [Mtro].[Distrito] AS MDT ON MP.Id = MDT.IdProvincia
	WHERE MDT.CodigoUbigeo = @CodUbigeo
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ObtenerDepartamento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ObtenerDepartamento]
	@CodUbigeo [varchar](6)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		 MD.[Descripcion]
	FROM [Mtro].[Pais] AS MPS INNER JOIN
		 [Mtro].[Departamento] AS MD ON MPS.Id = MD.IdPais INNER JOIN
		 [Mtro].[Provincia] AS MP ON MD.Id = MP.IdDepartamento INNER JOIN
		 [Mtro].[Distrito] AS MDT ON MP.Id = MDT.IdProvincia
	WHERE MDT.CodigoUbigeo = @CodUbigeo
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ObtenerDistrito]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ObtenerDistrito]
	@CodUbigeo [varchar](6)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		 MDT.[Descripcion]
	FROM [Mtro].[Pais] AS MPS INNER JOIN
		 [Mtro].[Departamento] AS MD ON MPS.Id = MD.IdPais INNER JOIN
		 [Mtro].[Provincia] AS MP ON MD.Id = MP.IdDepartamento INNER JOIN
		 [Mtro].[Distrito] AS MDT ON MP.Id = MDT.IdProvincia
	WHERE MDT.CodigoUbigeo = @CodUbigeo
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ObtenerProvincia]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ObtenerProvincia]
	@CodUbigeo [varchar](6)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		 MP.[Descripcion]
	FROM [Mtro].[Pais] AS MPS INNER JOIN
		 [Mtro].[Departamento] AS MD ON MPS.Id = MD.IdPais INNER JOIN
		 [Mtro].[Provincia] AS MP ON MD.Id = MP.IdDepartamento INNER JOIN
		 [Mtro].[Distrito] AS MDT ON MP.Id = MDT.IdProvincia
	WHERE MDT.CodigoUbigeo = @CodUbigeo
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ObtenerUbigeo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ObtenerUbigeo] --'150105'
	@CodigoUbigeo [varchar](6)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		MPS.[Id] AS IdPais,
		MPS.[Cod_Pais]
		,MD.[Id] AS IdDepartamento
		,MD.[Codigo]
		,MD.[Descripcion]
		,MP.[Id] AS IdProvincia
		,MP.[Codigo]
		,MP.[Descripcion]
		,MDT.[Id] AS IdDistrito
		,MDT.[CodigoUbigeo]
		,MDT.[Descripcion]
	FROM [Mtro].[Pais] AS MPS INNER JOIN
		 [Mtro].[Departamento] AS MD ON MPS.Id = MD.IdPais INNER JOIN
		 [Mtro].[Provincia] AS MP ON MD.Id = MP.IdDepartamento INNER JOIN
		 [Mtro].[Distrito] AS MDT ON MP.Id = MDT.IdProvincia
	WHERE MDT.CodigoUbigeo = @CodigoUbigeo
	
END 
GO
/****** Object:  StoredProcedure [Mtro].[Usp_Update_CtaBank]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Mtro].[Usp_Update_CtaBank]
@Id INT,
@Code VARCHAR(30),
@BankName VARCHAR(250),
@CtaSoles VARCHAR(50),
@CtaDolares VARCHAR(50),
@RucEntity VARCHAR(11),
@TypeBank INT,
@IdEstado INT
AS
BEGIN
	UPDATE [Mtro].[BankCta]
		SET 
		Code = @Code,
		BankName = @BankName,
		CtaSoles = @CtaSoles,
		CtaDolares = @CtaDolares,
		RucEntity = @RucEntity,
		TypeBank = @TypeBank,
		IdEstado = @IdEstado
	WHERE RucEntity = @RucEntity AND Id = @Id
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ValidarNroDocumentoCliente]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ValidarNroDocumentoCliente]
	@NroDocumento [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		*
	FROM [Mtro].[Cliente] AS MC
	WHERE MC.NroDocumento = @NroDocumento
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ValidarRucEmpresa]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ValidarRucEmpresa]
	@RUC [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		IdEmpresa
		,RUC
	FROM [Mtro].[Empresa]
	WHERE Ruc = @RUC
END


GO
/****** Object:  StoredProcedure [Mtro].[Usp_ValidarRucEmpresaData]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Mtro].[Usp_ValidarRucEmpresaData]
	@RUC [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		IdEmpresa
		,CodEmpresa
      ,Ubigeo
      ,Ruc
      ,RazonSocial
      ,RazonComercial
      ,Telefono
      ,Fax
      ,Direccion
      ,DomicilioFiscal
      ,Urbanizacion
      ,PaginaWeb
      ,Email
	FROM [Mtro].[Empresa]
	WHERE Ruc = @RUC
END


GO
/****** Object:  StoredProcedure [Seg].[Usp_ActualizarContrasenia]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ActualizarContrasenia]
	@IdUsuario [int],
	@Password [varchar](250),
	@NuevoPassword [varchar](250)
WITH EXECUTE AS CALLER
AS
BEGIN
	UPDATE [Seg].[Usuarios]
		SET [Password] = @NuevoPassword
	WHERE IdUsuario = @IdUsuario
		  --[Password] = @Password
END
GO
/****** Object:  StoredProcedure [Seg].[Usp_ActualizarCorreo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ActualizarCorreo]
	@IdEmail [int],
	@IdEmpresa [int],
	@Email [varchar](150),
	@Password [varchar](100),
	@DOMAIN [varchar](250),
	@IP [varchar](30),
	@PORT [int],
	@RucEmpresa [varchar](11),
	@IDESTADO [int],
	@TypeMail [varchar](10),
	@UseSSL [int]
WITH EXECUTE AS CALLER
AS
--DECLARE @RucEmpresa VARCHAR(11)
--DECLARE @RazonSocial VARCHAR(250)
--DECLARE @Direccion VARCHAR(250)
--DECLARE @Telefono VARCHAR(20)
--DECLARE @NUMBER INT
BEGIN
	UPDATE [Seg].[Correo] 
		SET [Email] = @Email,
			[Password] = @Password,
			[DOMAIN] = @DOMAIN,
			[IP] = @IP,
			[PORT]=@PORT,
			[IdEstado]=@IdEstado,
			[TypeMail] = @TypeMail,
			[UseSSL] = @UseSSL
	WHERE IdEmail = @IdEmail AND
		  IdEmpresa = @IdEmpresa AND
		  RucEmpresa = @RucEmpresa
END
--BEGIN
--	EXEC @RucEmpresa = [Mtro].[fn_GetRucEmpresaBs] @IdEmpresa
--	EXEC @RazonSocial = [Mtro].[fn_GetRazonSocialEmpresaBs] @IdEmpresa
--	EXEC @Direccion = [Mtro].[fn_GetDireccionEmpresaBs] @IdEmpresa
--	EXEC @Telefono = [Mtro].[fn_GetTelefonoEmpresaBs] @IdEmpresa
--	EXEC @NUMBER = [BD_FacturacionPortal].[Seg].[Usp_UpdateEmpresaBs] @IdEmpresa,@RucEmpresa,@RazonSocial,@Direccion,@Telefono,@Email,@Password
--END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ActualizarUsuario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ActualizarUsuario]
	@IdUsuario [int],
	@IdEmpleado [int],
	@IdEstado [int],
	@IdEmpresa [int],
	@Username [varchar](250),
	@Password [varchar](250),
	@FechaRegistro [datetime]
WITH EXECUTE AS CALLER
AS
BEGIN
	UPDATE [Seg].[Usuario] SET 
		IdEmpleado = @IdEmpleado
		,IdEstado = @IdEstado
		,IdEmpresa = @IdEmpresa
		--,CodEmpleado = @CodEmpleado
		,Username = @Username
		,Password = @Password
	WHERE IdUsuario = @IdUsuario
END


GO
/****** Object:  StoredProcedure [Seg].[Usp_ActualizarUsuarios]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ActualizarUsuarios]
	@IdUsuario [int],
	@IdEstado [int],
	@IdEmpresa [int],
	@IdPerfil [int],
	@Nombres [varchar](100),
	@ApePaterno [varchar](100),
	@ApeMaterno [varchar](100),
	@DNI_RUC [varchar](30),
	@Direccion [varchar](200),
	@Telefono [varchar](15),
	@Email [varchar](250),
	@Username [varchar](250),
	@Password [varchar](250),
	@FechaExpiracion [datetime],
	@NameSede VARCHAR(200)
WITH EXECUTE AS CALLER
AS
BEGIN
	UPDATE [Seg].[Usuarios] SET
				IdEstado = @IdEstado, 
				IdEmpresa = @IdEmpresa, 
				IdPerfil = @IdPerfil,
				Nombres = @Nombres,
				ApePaterno = @ApePaterno,
				ApeMaterno = @ApeMaterno,
				--DNI_RUC = @DNI_RUC,
				Direccion = @Direccion,
				Telefono = @Telefono,
				Email = @Email,
				Username = @Username,
				[Password] = @Password,
				FechaExpiracion = @FechaExpiracion,
				--FechaRegistro = @FechaRegistro
				Sede = @NameSede
	WHERE IdUsuario = @IdUsuario AND
		  DNI_RUC = @DNI_RUC
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_BloquearUsuario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_BloquearUsuario]
	@Username [varchar](250)
WITH EXECUTE AS CALLER
AS
BEGIN
	UPDATE [Seg].[Usuarios] SET IdEstado = 3
	WHERE [Username] = @Username
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_Delete_MailAlert]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [Seg].[Usp_Delete_MailAlert]
@RucComp VARCHAR(11),
@Email VARCHAR(MAX),
@TypeMail Varchar(1)
AS
BEGIN
	DELETE FROM [Conf].[MailNotifications] 
	WHERE Correos = @Email AND RucEntity = @RucComp AND TypeMail = @TypeMail
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_DeleteCorreo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_DeleteCorreo]
	@IdEmail [int],
	@IdEmpresa [int],
	@Email [varchar](250)
WITH EXECUTE AS CALLER
AS
BEGIN
	DELETE FROM [Seg].[Correo]
	WHERE IdEmail = @IdEmail AND
		  IdEmpresa = @IdEmpresa
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_DeleteMenuPerfil]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_DeleteMenuPerfil]
	@IdMenu [int],
	@IdPerfil [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	DELETE FROM [Seg].[MenuPerfil]
	WHERE IdMenu = @IdMenu AND
		  IdPerfil = @IdPerfil
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_DeletePerfil]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_DeletePerfil]
	@IdPerfil [int],
	@RucEntity [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	DELETE FROM [Seg].[Perfil]
	WHERE IdPerfil = @IdPerfil AND RucEntity = @RucEntity
END






GO
/****** Object:  StoredProcedure [Seg].[Usp_DeletePerfilUsuario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_DeletePerfilUsuario]
	@IdUsuario [int],
	@IdPerfil [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	DELETE FROM [Seg].[UsuarioPerfil]
	WHERE [IdUsuario] = @IdUsuario AND
		  [IdPerfil] = @IdPerfil
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_DeleteProfile]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_DeleteProfile]
@IdProf INT,
@Cod VARCHAR(100),
@RucComp VARCHAR(11)
AS
BEGIN
	DELETE FROM [Seg].[MenuPerfil] WHERE IdPerfil = @IdProf
	DELETE FROM [Seg].[Perfil] WHERE IdPerfil = @IdProf AND Codigo = @Cod AND RucEntity = @RucComp
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_DeleteRegistroLogueo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_DeleteRegistroLogueo]
@FechaDesde VARCHAR(10),
@RucEntity VARCHAR(11)
AS
BEGIN
	DELETE FROM [Seg].[RegistroLogueo]
	WHERE RucEntity = @RucEntity
	AND FECHA < @FechaDesde
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_DeleteUsuarioRol]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_DeleteUsuarioRol]
	@Dni [varchar](20),
	@IdRol [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	DELETE FROM 
		[Seg].[UsuarioRol]
	WHERE [Dni] = @Dni AND
		  [IdRol] = @IdRol
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_GetAmbienteTrbjActual]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_GetAmbienteTrbjActual]
@RUCENTITY VARCHAR(11)
AS
BEGIN
	SELECT
		[ID],
		CTAT.[COD],
		CTAT.[DESCRIPCION],
		CATA.[IDAMBIENTE]
	FROM [Conf].[Tpo_Amb_Trabj] AS CTAT INNER JOIN
		 [Conf].[AmbienteTrabj] AS CATA ON CTAT.[IDAMBIENTE] = CATA.IDAMBIENTE
	WHERE CATA.[RUCENTITY] = @RUCENTITY
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_GetDataFromUserLogueo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_GetDataFromUserLogueo]
@Username VARCHAR(30)
AS
BEGIN
	SELECT 
		[IdUsuario],
		[DNI_RUC] AS DNI,
		[IdPerfil]
	FROM [Seg].[Usuarios]
	WHERE [Username] = @Username
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_GetEntitySetup]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_GetEntitySetup]
@EntityId VARCHAR(11)
AS
BEGIN
	SELECT
		[IdEmpresa] AS EntityId,
		[Ruc] AS RucEntity,
		[RazonSocial] AS RazonSocial
	FROM [Mtro].[Empresa]
	WHERE [Ruc] = @EntityId
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_GetKySum41]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_GetKySum41]
	@Parametro1 [varchar](max),
	@Parametro2 [varchar](max),
	@Parametro3 [varchar](max),
	@Parametro4 [varchar](max)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT
		--KY AS 'Ky',
		--[ENCRYPT] AS 'KySum41'
		Id
	FROM [Seg].[SETUP]
	WHERE [COD_HD] = @Parametro1 AND
		  [COD_MB] = @Parametro2 AND
		  [COD_MC] = @Parametro3 AND
		  [ENCRYPT] = @Parametro4
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_GetList_NotificationsMail]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_GetList_NotificationsMail]
@ruccomp VARCHAR(11)
AS
BEGIN
	SELECT
		RucEntity,
		Correos AS Email,
		ContactName,
		CASE TypeMail
		WHEN 'F' THEN 'Facturación'
		WHEN 'S' THEN 'Soporte'
		END AS TypeMail
	FROM [Conf].[MailNotifications]
	WHERE [RucEntity] = @ruccomp
END



GO
/****** Object:  StoredProcedure [Seg].[Usp_GetList_ProfileCompany]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_GetList_ProfileCompany]
@IdComp INT
AS
BEGIN
	SELECT 
		SP.[IdPerfil],
		SP.[NombrePerfil] AS NombrePerfil,
		SP.[Codigo] AS Codigo,
		SP.[RucEntity]
	FROM [Seg].[Perfil] AS SP INNER JOIN
	[Mtro].[Empresa] AS ME ON SP.RucEntity = ME.Ruc
	WHERE ME.IdEmpresa = @IdComp
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_GetListAmbiente]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_GetListAmbiente]
AS
BEGIN
	SELECT 
		CTA.[IDAMBIENTE],
		CTA.[COD],
		CTA.[DESCRIPCION],
		CTA.[IDESTADO],
		ME.[Descripcion] AS EstadoDescripcion
	FROM [Mtro].[Estado] AS ME INNER JOIN
		 [Conf].[Tpo_Amb_Trabj] AS CTA ON ME.IdEstado = CTA.IDESTADO
	END

GO
/****** Object:  StoredProcedure [Seg].[Usp_GetListLogAde]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [Seg].[Usp_GetListLogAde]
@IdMenu INT,
@FechaDesde DATETIME,
@FechaHasta DATETIME
AS
BEGIN
	SELECT
		SLA.ID,
		SLA.IdMenu,
		SM.NombreMenu,
		SLA.MenuAmbiente,
		SLA.MessageLog,
		SLA.InnerException,
		SLA.Fecha,
		SLA.Correction
	FROM [Seg].[Menu] AS SM INNER JOIN 
		 [Seg].[LogAde] AS SLA ON SM.IdMenu = SLA.IdMenu
	WHERE (@IdMenu = 0 OR SLA.IdMenu = @IdMenu) AND
		  (@FechaDesde = '' AND @FechaHasta = '') OR (SLA.Fecha BETWEEN @FechaDesde AND @FechaHasta)
END
GO
/****** Object:  StoredProcedure [Seg].[Usp_GetListLogLogueo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [Seg].[Usp_GetListLogLogueo]
@FechaDesde DATETIME,
@FechaHasta DATETIME
AS
BEGIN
	SELECT 
		SLL.ID,
		FECHA AS Fecha,
		USERNAME AS Username,
		USERNAME_LOG AS Username_Log,
		IDTIPOLOG AS IdTipoLog,
		STL.DESCRIPCION AS Descripcion
	FROM [Seg].[TipoLog] AS STL INNER JOIN
		 [Seg].[LogLogueo] AS SLL ON STL.ID = SLL.IDTIPOLOG
	WHERE --SLL.FECHA BETWEEN @FechaDesde AND @FechaHasta
		  (@FechaDesde = '' AND @FechaHasta = '') OR (SLL.FECHA BETWEEN @FechaDesde AND @FechaHasta)
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_GetListRegistroLogueo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_GetListRegistroLogueo] --'8/08/2016','15/08/2016',0,'20101071562'
@FechaDesde VARCHAR(10),
@FechaHasta VARCHAR(10),
@IdTipoLog INT,
@RucEntity VARCHAR(11),
@IdPerfil INT,
@Username VARCHAR(50)
AS
BEGIN
	SELECT
		--SRL.ID,
		CONVERT(INT, ROW_NUMBER() OVER(ORDER BY SRL.ID)) AS ID,
		SRL.DNI_USUARIO AS DNI,
		SRL.IDPERFIL AS IdPerfil,
		ISNULL(SP.[NombrePerfil], 'Sin Perfil') AS NombrePerfil,
		SRL.USERNAME AS Username,
		ISNULL(SU.[Nombres] + ' ' + SU.[ApePaterno] + ' ' + SU.[ApeMaterno], 'Sin Nombre') AS NombresApellidos,
		SRL.FECHAINGRESO AS FechaIngreso,
		SRL.FECHASALIDA AS FechaSalida,
		SRL.HOSTNAME AS HostName,
		SRL.SIP AS sIP,
		SRL.IDTPOLOG AS IdTipoLog,
		ST.DESCRIPCION AS Descripcion
	FROM [Seg].[TipoLog] AS ST INNER JOIN
		 [Seg].[RegistroLogueo] AS SRL ON ST.ID = SRL.IDTPOLOG LEFT JOIN
		 [Seg].[Perfil] AS SP ON SRL.IDPERFIL = SP.IdPerfil LEFT JOIN
		 [Seg].[Usuarios] AS SU ON SRL.USERNAME = SU.Username
	WHERE 
		  (@FechaDesde = '' AND @FechaHasta = '' OR SRL.FECHA BETWEEN @FechaDesde AND @FechaHasta) AND
		  (@IdTipoLog = 0 OR ST.ID = @IdTipoLog) AND
		  (SRL.RucEntity = @RucEntity) AND
		  (@IdPerfil = 0 OR SRL.IDPERFIL = @IdPerfil) AND
		  (@Username = '' OR SRL.USERNAME LIKE @Username + '%')
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_GetListTipoLog]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_GetListTipoLog]
AS
BEGIN
	SELECT
		[ID],
		[COD] AS Codigo,
		[DESCRIPCION] AS Descripcion
	FROM [Seg].[TipoLog]
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_GetStatus_WService]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_GetStatus_WService] --'20547025319', 'ADE.Services.SendMail'
@Ruc VARCHAR(11),
@NameService VARCHAR(150)
AS
BEGIN
	SELECT * 
	FROM [Conf].[TimeService]
	WHERE (CodeService = @NameService) AND
		  (RucEntity = @Ruc) AND (IdEstado = 1)
		  
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_GetTimeForExeProccessMD]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_GetTimeForExeProccessMD] --'ADE.Services.DownloadDocument'
@CodeService VARCHAR(50)
--@RucEntity VARCHAR(11)
AS
BEGIN
	SELECT
		*
	FROM [Conf].[TimeService]
	WHERE [CodeService] = @CodeService --AND [RucEntity] = @RucEntity
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_IniciarSesion]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_IniciarSesion] --'admin','z75H87aIz0RWw9blGx1AQaotbMnSekwfkg8+u1cJ45o=',29,''
	@Username [varchar](250),
	@Password [varchar](250),
	@IdEmpresa [int],
	@TpoLogin [varchar](20)
WITH EXECUTE AS CALLER
AS
--IF @TpoLogin = 'LDAP'
IF @TpoLogin != '-'
BEGIN
		SELECT *
		FROM [Seg].[Usuarios] AS SU
		WHERE SU.Username = @Username AND
			  --SU.[Password] = @Password AND
			  --SU.IdEstado = 1 AND
			  SU.IdEmpresa = @IdEmpresa
	END
ELSE
BEGIN
		SELECT *
		FROM [Seg].[Usuarios] AS SU
		WHERE SU.Username = @Username AND
			  SU.[Password] = @Password AND
			  --SU.IdEstado = 1 AND
			  SU.IdEmpresa = @IdEmpresa
	END

GO
/****** Object:  StoredProcedure [Seg].[Usp_Insert_MailAlert]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_Insert_MailAlert]
@RucComp VARCHAR(11),
@Email VARCHAR(MAX),
@ContactName VARCHAR(250),
@Email_Old VARCHAR(MAX),
@TypeMail VARCHAR(1)
AS
BEGIN
IF @Email_Old = '1'
	IF EXISTS (SELECT * FROM [Conf].[MailNotifications] WHERE Correos = @Email AND RucEntity = @RucComp AND TypeMail = @TypeMail)
		BEGIN
			UPDATE [Conf].[MailNotifications] SET Correos = @Email, ContactName = @ContactName, TypeMail = @TypeMail
			WHERE Correos = @Email_Old AND RucEntity = @RucComp
		END
	ELSE
		BEGIN
			INSERT INTO [Conf].[MailNotifications]
			VALUES (@RucComp, @Email, @ContactName, @TypeMail)
		END
ELSE
	IF EXISTS (SELECT * FROM [Conf].[MailNotifications] WHERE Correos = @Email_Old AND RucEntity = @RucComp AND TypeMail = @TypeMail)
		BEGIN
			UPDATE [Conf].[MailNotifications] SET Correos = @Email, ContactName = @ContactName, TypeMail = @TypeMail
			WHERE Correos = @Email_Old AND RucEntity = @RucComp
		END
	ELSE
		BEGIN
			INSERT INTO [Conf].[MailNotifications]
			VALUES (@RucComp, @Email, @ContactName, @TypeMail)
		END
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_InsertarCorreo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_InsertarCorreo]
	@IdEmpresa [int],
	@Email [varchar](150),
	@Password [varchar](100),
	@DOMAIN [varchar](250),
	@IP [varchar](30),
	@PORT [Int],
	@RucEmpresa [varchar](11),
	@IDESTADO [Int],
	@TypeMail [varchar](10),
	@UseSSL [int]
--WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Seg].[Correo]
			([IdEmpresa], [Email], [Password],[DOMAIN], [IP], [PORT], [IdEstado], [RucEmpresa], [TypeMail], [UseSSL])
		VALUES
			(@IdEmpresa, @Email, @Password, @DOMAIN, @IP, @PORT, @IDESTADO, @RucEmpresa, @TypeMail, @UseSSL)
END
	
--DECLARE @RucEmpresa VARCHAR(11)
--DECLARE @RazonSocial VARCHAR(250)
--DECLARE @Direccion VARCHAR(250)
--DECLARE @Telefono VARCHAR(20)
--DECLARE @NUMBER INT
--IF NOT EXISTS (SELECT * FROM [Seg].[Correo] WHERE [IdEmpresa] = @IdEmpresa AND [Email] = @Email)
--	BEGIN
--		INSERT INTO [Seg].[Correo]
--			([IdEmpresa], [Email], [Password])
--		VALUES
--			(@IdEmpresa, @Email, @Password)
--	END
--BEGIN
--	EXEC @RucEmpresa = [Mtro].[fn_GetRucEmpresaBs] @IdEmpresa
--	EXEC @RazonSocial = [Mtro].[fn_GetRazonSocialEmpresaBs] @IdEmpresa
--	EXEC @Direccion = [Mtro].[fn_GetDireccionEmpresaBs] @IdEmpresa
--	EXEC @Telefono = [Mtro].[fn_GetTelefonoEmpresaBs] @IdEmpresa
--	EXEC @NUMBER = [BD_FacturacionPortal].[Seg].[Usp_RegistraEmpresaBs] @RucEmpresa,@RazonSocial,@Direccion,@Telefono,@Email,@Password
--END

GO
/****** Object:  StoredProcedure [Seg].[Usp_InsertarMenuPerfil]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_InsertarMenuPerfil]
	@IdMenu [int],
	@IdPerfil [int]
WITH EXECUTE AS CALLER
AS
IF NOT EXISTS (SELECT IdMenu, IdPerfil FROM [Seg].[MenuPerfil] WHERE IdMenu = @IdMenu AND IdPerfil = @IdPerfil)
BEGIN
	INSERT INTO [Seg].[MenuPerfil]
				(IdMenu, IdPerfil)
				VALUES
				(@IdMenu, @IdPerfil)
	END
	ELSE
BEGIN
	UPDATE [Seg].[MenuPerfil] SET IdMenu = @IdMenu, IdPerfil = @IdPerfil
	WHERE IdMenu = @IdMenu AND IdPerfil = @IdPerfil
	END

GO
/****** Object:  StoredProcedure [Seg].[Usp_InsertarPerfil]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_InsertarPerfil]
	@NombrePerfil [varchar](100),
	@Codigo [varchar](100),
	@RucEntity [varchar](100)
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Seg].[Perfil]
				(NombrePerfil
				,Codigo
				,RucEntity)
				VALUES
				(@NombrePerfil
				,@Codigo
				,@RucEntity)
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_InsertarUsuarios]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_InsertarUsuarios]
	@IdEstado [int],
	@IdEmpresa [int],
	@IdPerfil [int],
	@Nombres [varchar](100),
	@ApePaterno [varchar](100),
	@ApeMaterno [varchar](100),
	@DNI_RUC [varchar](30),
	@Direccion [varchar](200),
	@Telefono [varchar](15),
	@Email [varchar](100),
	@Username [varchar](50),
	@Password [varchar](max),
	@FechaExpiracion [datetime],
	@FechaRegistro [datetime],
	@NameSede VARCHAR(200)
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Seg].[Usuarios]
				(IdEstado, IdEmpresa, IdPerfil,
				Nombres, ApePaterno, ApeMaterno,
				DNI_RUC, Direccion, Telefono,
				Email, Username, [Password],
				FechaExpiracion, FechaRegistro, Sede)
				output inserted.IdUsuario
				VALUES
				(@IdEstado, @IdEmpresa, @IdPerfil,
				@Nombres, @ApePaterno, @ApeMaterno,
				@DNI_RUC, @Direccion, @Telefono, @Email,
				@Username, @Password,@FechaExpiracion,
				@FechaRegistro, @NameSede)
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_InsertarUsuarios_Excel]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_InsertarUsuarios_Excel]
	@IdEstado [int],
	@IdEmpresa [int],
	@IdPerfil [int],
	@Nombres [varchar](100),
	@ApePaterno [varchar](100),
	@ApeMaterno [varchar](100),
	@DNI_RUC [varchar](30),
	@Direccion [varchar](200),
	@Telefono [varchar](15),
	@Email [varchar](100),
	@Username [varchar](30),
	@Password [varchar](250),
	@FechaExpiracion [datetime],
	@FechaRegistro [datetime]
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Seg].[Usuarios]
		(IdEstado, IdEmpresa, IdPerfil,
		Nombres, ApePaterno, ApeMaterno,
		DNI_RUC, Direccion, Telefono,
		Email, Username, [Password],
		FechaExpiracion, FechaRegistro)
		VALUES
		(@IdEstado, @IdEmpresa, @IdPerfil,
		@Nombres, @ApePaterno, @ApeMaterno,
		@DNI_RUC, @Direccion, @Telefono, @Email,
		@Username, @Password, @FechaExpiracion,
		@FechaRegistro)
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_InsertLogAde]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [Seg].[Usp_InsertLogAde]
@IdMenu INT,
@MenuAmbiente VARCHAR(60),
@MessageLog VARCHAR(MAX),
@InnerException VARCHAR(MAX),
@Fecha DATETIME,
@Correction VARCHAR(MAX)
AS
BEGIN
	INSERT INTO [Seg].[LogAde]
		(IdMenu, MenuAmbiente, MessageLog,
		InnerException, Fecha, Correction)
		VALUES
		(@IdMenu, @MenuAmbiente, @MessageLog,
		@InnerException, @Fecha, @Correction)
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_InsertLogLogueo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_InsertLogLogueo]
@FECHA DATETIME,
@USERNAME_LOG VARCHAR(50),
@IDTIPOLOG INT
AS
BEGIN
	INSERT INTO [Seg].[LogLogueo]
		(FECHA, USERNAME_LOG, IDTIPOLOG)
		VALUES
		(@FECHA, @USERNAME_LOG, @IDTIPOLOG)
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_InsertRegistroLogueo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_InsertRegistroLogueo]
@FECHA DATETIME,
@DNI_USUARIO VARCHAR(11),
@USERNAME VARCHAR(50),
@IDPERFIL INT,
@FECHAINGRESO DATETIME,
@HOSTNAME VARCHAR(250),
@SIP VARCHAR(30),
@IDTPOLOG INT,
@IdEmpresa INT
AS
BEGIN
	INSERT INTO [Seg].[RegistroLogueo]
		(FECHA, DNI_USUARIO, USERNAME, 
		IDPERFIL, FECHAINGRESO, HOSTNAME, SIP, IDTPOLOG, RucEntity)
		OUTPUT inserted.ID
		VALUES
		(@FECHA, @DNI_USUARIO, @USERNAME,
		@IDPERFIL, @FECHAINGRESO, @HOSTNAME, @SIP, @IDTPOLOG, (SELECT Ruc FROM [Mtro].[Empresa] WHERE IdEmpresa = @IdEmpresa))
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ListaCorreo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_ListaCorreo] --9,'','',0
@IdEmpresa INT,
@RazonSocial VARCHAR(100),
@Email VARCHAR(150),
@IdEstado INT
AS
BEGIN
	SELECT 		
		SC.IdEmail,
		--CONVERT(INT, ROW_NUMBER() OVER(ORDER BY SC.IdEmail)) AS IdEmail,
		ME.IdEmpresa,
		ME.RazonSocial,
		SC.Email,
		SC.[Password] AS 'Password',
		SC.[DOMAIN] AS Domain ,
		SC.[IP] AS IP,
		SC.[PORT] AS Port,
		SC.[IdEstado],
		MS.Descripcion,
		SC.[RucEmpresa] AS RUC,
		SC.[TypeMail],
		SC.[UseSSL]
	FROM [Seg].[Correo] AS SC INNER JOIN
		 [Mtro].[Empresa] AS ME ON SC.IdEmpresa = ME.IdEmpresa INNER JOIN
		 [Mtro].[Estado] AS MS ON SC.IdEstado = MS.IdEstado
	WHERE (@IdEmpresa = 0 OR ME.IdEmpresa = @IdEmpresa) AND
		  (@RazonSocial = '' OR ME.RazonSocial LIKE @RazonSocial + '%') AND
		  (@Email = '' OR SC.Email LIKE @Email + '%' ) AND
		  (@IdEstado = 0 OR ME.IdEstado = @IdEstado)
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ListadoPerfiles]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ListadoPerfiles]
@RucEntity [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT [IdPerfil],
		   [NombrePerfil] AS NombrePerfil,
		   [Codigo] AS Codigo,
		   [RucEntity]
	FROM   [Seg].[Perfil]
	WHERE RucEntity = @RucEntity
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ListadoRoles]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [Seg].[Usp_ListadoRoles]
AS
BEGIN
	SELECT
		SR.[IdRol]
		,SR.[NombreRol]
		,SR.[CodigoRol]
	FROM [Seg].[Rol] AS SR
	WHERE [IdEstado] = 1
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ListaPerfil]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ListaPerfil]
	@NombrePerfil [varchar](50),
	@RucEntity [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT [IdPerfil]
		   ,[NombrePerfil]
		   ,[Codigo]
		   ,[RucEntity]
	FROM [Seg].[Perfil]
	WHERE (@NombrePerfil = '' OR NombrePerfil LIKE @NombrePerfil + '%') 
		  AND RucEntity = @RucEntity

END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ListarMenu]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ListarMenu]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT  SM.IdMenu, 
			SM.NombreMenu, 
			SM.CodigoMenu,
			SM.CodigoMenu, 
		    SM.PadreMenu,
		    SM.NivelMenu,
		    SM.IdEstado,
		    ME.Descripcion
	FROM    Seg.Menu AS SM INNER JOIN
		[Mtro].[Estado] AS ME ON SM.IdEstado = ME.IdEstado
	WHERE SM.IdEstado = 1
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ListarMenuPerfil]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ListarMenuPerfil]
	@IdPerfil [int],
	@RucEntity [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT Seg.Perfil.IdPerfil, 
		   Seg.Menu.IdMenu, 
		   Seg.Menu.NombreMenu, 
		   Seg.Menu.CodigoMenu, 
		   Seg.Menu.PadreMenu,
		   Seg.Menu.NivelMenu,
		   Seg.Menu.IdEstado,
		   Mtro.Estado.Descripcion,
		   Seg.Perfil.RucEntity
	FROM   Seg.Menu INNER JOIN
		   Seg.MenuPerfil ON Seg.Menu.IdMenu = Seg.MenuPerfil.IdMenu INNER JOIN
		   Seg.Perfil ON Seg.MenuPerfil.IdPerfil = Seg.Perfil.IdPerfil INNER JOIN
		   Mtro.Estado ON Seg.Menu.IdEstado = Mtro.Estado.IdEstado
	WHERE Seg.Perfil.IdPerfil = @IdPerfil
		  AND Seg.Perfil.RucEntity = @RucEntity



	--SELECT 
	--	   Seg.Usuarios.IdPerfil,
	--	   Seg.Menus.IdMenu, 
	--	   Seg.Menus.NombreMenu, 
	--	   Seg.Menus.CodigoMenu, 
	--	   Seg.Menus.PadreMenu,
	--	   Seg.Menus.NivelMenu,
	--	   Seg.Menus.IdEstado,
	--	   Mtro.Estado.Descripcion
	--FROM   Seg.Menus LEFT JOIN
	--	   Seg.MenuPerfil ON Seg.Menus.IdMenu = Seg.MenuPerfil.IdMenu LEFT JOIN
	--	   Seg.Perfil ON Seg.MenuPerfil.IdPerfil = Seg.Perfil.IdPerfil LEFT JOIN
	--	   Seg.UsuarioPerfil ON Seg.Perfil.IdPerfil = Seg.UsuarioPerfil.IdPerfil LEFT JOIN
	--	   Seg.Usuarios ON Seg.Perfil.IdPerfil = Seg.Usuarios.IdPerfil LEFT JOIN
	--	   Mtro.Estado ON Seg.Menus.IdEstado = Mtro.Estado.IdEstado

	ORDER BY Seg.Menu.IdMenu
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ListarMenuPerfilUsuario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ListarMenuPerfilUsuario] --1
	@IdUsuario [int],
	@RucEntity [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	--SELECT 
	--	   Seg.Usuarios.IdPerfil,
	--	   Seg.Menu.IdMenu, 
	--	   Seg.Menu.NombreMenu, 
	--	   Seg.Menu.CodigoMenu--, 
	--	   --Seg.MenuPerfil.IdPerfil, 
	--	   --Seg.Perfil.NombrePerfil, 
	--	   --Seg.Perfil.Codigo, 
	--	   --Seg.Usuario.IdUsuario 
	--FROM   Seg.Menu INNER JOIN
	--	   Seg.MenuPerfil ON Seg.Menu.IdMenu = Seg.MenuPerfil.IdMenu LEFT JOIN
	--	   Seg.Perfil ON Seg.MenuPerfil.IdPerfil = Seg.Perfil.IdPerfil LEFT JOIN
	--	   Seg.UsuarioPerfil ON Seg.Perfil.IdPerfil = Seg.UsuarioPerfil.IdPerfil LEFT JOIN
	--	   Seg.Usuarios ON Seg.Perfil.IdPerfil = Seg.Usuarios.IdPerfil
	--WHERE Seg.Usuarios.IdUsuario = @IdUsuario



	SELECT 
		   Seg.Usuarios.IdPerfil,
		   Seg.Menu.IdMenu, 
		   --(' ' + Seg.Menu.NombreMenu) AS NombreMenu,
		   Seg.Menu.NombreMenu,
		   Seg.Menu.CodigoMenu, 
		   Seg.Menu.PadreMenu,
		   Seg.Menu.NivelMenu,
		   Seg.Menu.IdEstado,
		   Mtro.Estado.Descripcion,
		   --Seg.MenuPerfil.IdPerfil, 
		   --Seg.Perfil.NombrePerfil, 
		   --Seg.Perfil.Codigo, 
		   --Seg.Usuario.IdUsuario
		   Seg.Perfil.RucEntity
	FROM   Seg.Menu INNER JOIN
		   Seg.MenuPerfil ON Seg.Menu.IdMenu = Seg.MenuPerfil.IdMenu INNER JOIN
		   Seg.Perfil ON Seg.MenuPerfil.IdPerfil = Seg.Perfil.IdPerfil INNER JOIN
		   --Seg.UsuarioPerfil ON Seg.Perfil.IdPerfil = Seg.UsuarioPerfil.IdPerfil LEFT JOIN
		   Seg.Usuarios ON Seg.Perfil.IdPerfil = Seg.Usuarios.IdPerfil INNER JOIN
		   Mtro.Estado ON Seg.Menu.IdEstado = Mtro.Estado.IdEstado
	WHERE Seg.Usuarios.IdUsuario = @IdUsuario AND
		  Seg.Menu.IdEstado = 1 AND Seg.Perfil.RucEntity = @RucEntity
		  
	ORDER BY Seg.Menu.IdMenu
END
GO
/****** Object:  StoredProcedure [Seg].[Usp_ListarMenusPrueba]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ListarMenusPrueba]
	@IdUsuario [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		   Seg.Usuarios.IdPerfil,
		   Seg.Menu.IdMenu, 
		   Seg.Menu.NombreMenu, 
		   Seg.Menu.CodigoMenu, 
		   Seg.Menu.PadreMenu,
		   Seg.Menu.NivelMenu,
		   Seg.Menu.IdEstado,
		   Mtro.Estado.Descripcion
		   --Seg.MenuPerfil.IdPerfil, 
		   --Seg.Perfil.NombrePerfil, 
		   --Seg.Perfil.Codigo, 
		   --Seg.Usuario.IdUsuario 
	FROM   Seg.Menu LEFT JOIN
		   Seg.MenuPerfil ON Seg.Menu.IdMenu = Seg.MenuPerfil.IdMenu LEFT JOIN
		   Seg.Perfil ON Seg.MenuPerfil.IdPerfil = Seg.Perfil.IdPerfil LEFT JOIN
		   Seg.UsuarioPerfil ON Seg.Perfil.IdPerfil = Seg.UsuarioPerfil.IdPerfil LEFT JOIN
		   Seg.Usuarios ON Seg.Perfil.IdPerfil = Seg.Usuarios.IdPerfil LEFT JOIN
		   Mtro.Estado ON Seg.Menu.IdEstado = Mtro.Estado.IdEstado
	WHERE Seg.Usuarios.IdUsuario = @IdUsuario
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ListarPerfilUsuario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ListarPerfilUsuario]
	@IdUsuario [int],
	@RucEntity [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT --SU.[IdUsuario],
		   SUP.IdUsuarioPerfil AS IdUsuarioPerfil,
		   SP.[IdPerfil],
		   SP.[NombrePerfil] AS NombreMenu,
		   SP.[Codigo] AS CodigoMenu,
		   SP.[RucEntity]
	FROM [Seg].[Usuarios] AS SU INNER JOIN
		 [Seg].[UsuarioPerfil] AS SUP ON SU.IdUsuario = SUP.IdUsuario INNER JOIN
		 [Seg].[Perfil] AS SP ON SUP.IdPerfil = SP.IdPerfil
	WHERE (@IdUsuario = 0 OR SU.IdUsuario = @IdUsuario) AND SP.RucEntity = @RucEntity
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ListarRolUsuario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ListarRolUsuario]
	@Dni varchar(30)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT --SU.[IdUsuario],
		   SR.[IdRol],
		   SR.[NombreRol],
		   SR.[CodigoRol]
	FROM [Seg].[Usuarios] AS SU LEFT JOIN
		 [Seg].[UsuarioRol] AS SUR ON SU.DNI_RUC = SUR.Dni LEFT JOIN
		[Seg].[Rol] AS SR ON SUR.IdRol = SR.IdRol
	WHERE SU.DNI_RUC = @Dni
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ListaUsuario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ListaUsuario]
	@Empresa [int],
	@DNI [varchar](20),
	@Username [varchar](30),
	@Estado [int],
	@RucEmpresa [varchar](11)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		CONVERT(INT,RANK() OVER (ORDER BY SU.IdUsuario)) AS Nro
		,SU.IdUsuario
		--,SU.IdEmpleado
		,SU.DNI_RUC AS DNI
		,SU.Nombres + ' ' + SU.ApePaterno + ' ' + SU.ApeMaterno AS Empleado
		,SU.Nombres + ' ' + SU.ApePaterno + ' ' + SU.ApeMaterno AS NombresApellidos
		,SU.Nombres
		,SU.ApePaterno
		,SU.ApeMaterno
		,SU.Username
		,SU.Password AS 'Password'
		,SU.Direccion
		,SU.Telefono
		,SU.Email
		,ME.IdEstado
		,ME.Descripcion
		,MEM.IdEmpresa
		,MEM.RazonSocial
		,MEM.Ruc
		,SP.IdPerfil
		,SP.NombrePerfil
		,SU.FechaExpiracion
		,ISNULL(SU.Sede, '') AS Sede
	FROM [Seg].[Usuarios] AS SU INNER JOIN
		 [Mtro].[Estado] AS ME ON SU.IdEstado = ME.IdEstado INNER JOIN
		 [Mtro].[Empresa] AS MEM ON SU.IdEmpresa = MEM.IdEmpresa INNER JOIN
		 [Seg].[Perfil] AS SP ON SU.IdPerfil = sp.IdPerfil

	WHERE (@Empresa = 0 OR MEM.IdEmpresa = @Empresa) AND
		  (@Estado = 0 OR ME.IdEstado = @Estado) AND
		  (@Username = '' OR SU.Username LIKE @Username + '%') AND
		  (@DNI = '' OR SU.DNI_RUC LIKE @DNI + '%') AND
		  (MEM.Ruc = @RucEmpresa) AND
		  (SU.DNI_RUC <> '88888888')
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ListSede]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [Seg].[Usp_ListSede] --'20216414056'
@RucEntity VARCHAR(11)
AS
BEGIN
	SELECT
		Id,
		Cod,
		Name,
		RucEntity
	FROM [Seg].[Sede]
	WHERE RucEntity = @RucEntity
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ObtenerUltimoIdUsuario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ObtenerUltimoIdUsuario]
WITH EXECUTE AS CALLER
AS
BEGIN
	--SELECT MAX(IdUsuario) + 1 AS IdUsuario
	SELECT MAX(IdUsuario) AS IdUsuario
	FROM [Seg].[UsuarioS]
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ObtenerUsuarioLogeado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ObtenerUsuarioLogeado] --'admin','psjHj4O/TB8='
	@Username [varchar](250),
	@Password [varchar](max)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		[IdUsuario]
		--,SU.[IdEmpleado]
		,SU.[IdEstado] IdEstadoUsuario
		,MES.[Descripcion] AS EstadoUsuario
		,SU.[Username]
		,SU.[Nombres] AS Nombres
		,SU.[ApePaterno] + ' ' + SU.[ApeMaterno] AS Apellidos
		,SU.[Dni_Ruc] as DNI
		,SU.Direccion 
		,SU.[Telefono]
		,SU.[Email]
		,SU.[Password] AS ClaveUsuario
		,MEP.[IdEmpresa]
		,MEP.[Ruc]
		,MEP.[RazonSocial]
		--,SC.[Email] AS Correo
		--,SC.[Password]
		,MEP.[PaginaWeb]
		,SP.IdPerfil
		,Sede AS Sede,
		SP.NombrePerfil,
		SP.Codigo
	FROM [Seg].[Usuarios] AS SU INNER JOIN
		 [Mtro].[Estado] AS MES ON SU.IdEstado = MES.IdEstado INNER JOIN
		 [Mtro].[Empresa] AS MEP ON SU.IdEmpresa = MEP.IdEmpresa INNER JOIN
		 --[Seg].[Correo] AS SC ON SU.IdEmpresa = SC.IdEmpresa INNER JOIN
		 [Seg].[Perfil] AS SP ON SU.IdPerfil = SP.IdPerfil
	WHERE SU.[Username] = @Username
		  --AND SU.[Password] = @Password
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_RegisterKySum41]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_RegisterKySum41]
	@COD_MB [varchar](max),
	@HOST [varchar](max),
	@COD_HD [varchar](max),
	@KY [varchar](max),
	@ENCRYPT [varchar](max),
	@COD_MC [varchar](max)
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Seg].[SETUP]
			(COD_MB, HOST, COD_HD, [KY], [ENCRYPT], [COD_MC])
			VALUES
			(@COD_MB, @HOST, @COD_HD, @KY, @ENCRYPT, @COD_MC)
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_RegistraEmpresaBs]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_RegistraEmpresaBs]
	@RucEmpresa [varchar](11),
	@RazonSocial [varchar](250),
	@Direccion [varchar](250),
	@Telefono [varchar](20),
	@Email [varchar](250),
	@Password [varchar](250)
WITH EXECUTE AS CALLER
AS
IF NOT EXISTS (SELECT * FROM [Mtro].[Empresa] WHERE RucEmpresa = @RucEmpresa)
	BEGIN
		INSERT INTO [Mtro].[Empresa]
			(RucEmpresa, RazonSocial,
			Direccion, Telefono,
			Email, [Password])
		VALUES
			(@RucEmpresa, @RazonSocial,
			@Direccion, @Telefono,
			@Email, @Password)
	END

GO
/****** Object:  StoredProcedure [Seg].[Usp_RegistrarPerfilUsuario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_RegistrarPerfilUsuario]
	@IdUsuario [int],
	@IdPerfil [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO Seg.UsuarioPerfil 
		(IdUsuario,
		IdPerfil) 
		VALUES 
		(@IdUsuario, @IdPerfil)
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_RegistrarUsuario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_RegistrarUsuario]
	@IdEmpleado [int],
	@IdEstado [int],
	@IdEmpresa [int],
	@DNI [varchar](30),
	@RUC [varchar](30),
	@Username [varchar](20),
	@Password [varchar](20),
	@FechaRegistro [datetime],
	@IdPerfil [int]
WITH EXECUTE AS CALLER
AS
BEGIN
	INSERT INTO [Seg].[Usuario](
		IdEmpleado, IdEstado, IdEmpresa
		,DNI, RUC ,Username, Password
		,FechaRegistro, IdPerfil)
	VALUES (
		@IdEmpleado, @IdEstado
		,@IdEmpresa
		,@DNI,@RUC
		,@Username, @Password
		,@FechaRegistro, @IdPerfil
		)
END


GO
/****** Object:  StoredProcedure [Seg].[Usp_RegistrarUsuarioRol]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_RegistrarUsuarioRol]
	@IdRol [int],
	@Dni [varchar](50)
WITH EXECUTE AS CALLER
AS
--BEGIN
--	INSERT INTO 
--		[Seg].[UsuarioRol]
--		([IdRol]
--		,[Dni])
--		VALUES 
--		(@IdRol
--		,@Dni)
--END
IF NOT EXISTS (SELECT [IdUsuarioRol] FROM [Seg].[UsuarioRol] WHERE [IdRol] = @IdRol AND [Dni] = @Dni)
	BEGIN
		INSERT INTO  [Seg].[UsuarioRol]
			([IdRol], [Dni])
			VALUES
			(@IdRol ,@Dni)
	END
ELSE
	BEGIN
		UPDATE [Seg].[UsuarioRol] SET [IdRol] = @IdRol
		WHERE [IdRol] = @IdRol AND [Dni] = @Dni
	END

GO
/****** Object:  StoredProcedure [Seg].[Usp_RegistrCompany_Ok]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_RegistrCompany_Ok]
@IdComp INT
AS
BEGIN
	SELECT
		*
	FROM [Mtro].[Empresa]
	WHERE IdEmpresa = @IdComp
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_RegistrPerfilComp]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_RegistrPerfilComp]
@Nomb VARCHAR(100),
@Cod VARCHAR(100),
@RucComp VARCHAR(11)
AS
BEGIN
	INSERT INTO [Seg].[Perfil]
	VALUES 
	(@Nomb, @Cod, @RucComp)
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_Update_MailAlert]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [Seg].[Usp_Update_MailAlert]
@RucComp VARCHAR(11),
@Email VARCHAR(MAX),
@ContactName VARCHAR(250)
AS
BEGIN
	UPDATE [Conf].[MailNotifications] SET Correos = @Email, ContactName = @ContactName
	WHERE Correos = @Email AND RucEntity = @RucComp
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_Update_UserCompany]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_Update_UserCompany]
@IdComp INT,
--@RucComp VARCHAR(11),
@IdUser INT,
@Ind VARCHAR(11),
@IdProf INT
AS
BEGIN
	UPDATE [Seg].[Usuarios] SET IdEmpresa = @IdComp, IdPerfil = @IdProf
	WHERE IdUsuario = @IdUser AND DNI_RUC = @Ind
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_UpdateAmbTrabjActual]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_UpdateAmbTrabjActual] --3
@IdAmbTrabj INT,
@RUCENTITY VARCHAR(11)
AS
IF NOT EXISTS (SELECT ID FROM [Conf].[AmbienteTrabj] WHERE RUCENTITY = @RUCENTITY)
	BEGIN
		INSERT INTO [Conf].[AmbienteTrabj] 
			(IDAMBIENTE, COD, DESCRIPCION, RUCENTITY)
		VALUES 
			(@IdAmbTrabj, (SELECT [COD] FROM [Conf].[Tpo_Amb_Trabj] WHERE [IDAMBIENTE] = @IdAmbTrabj)
			,(SELECT [DESCRIPCION] FROM [Conf].[Tpo_Amb_Trabj] WHERE [IDAMBIENTE] = @IdAmbTrabj)
			,@RUCENTITY)
	END
ELSE
	BEGIN
		UPDATE [Conf].[AmbienteTrabj]
			SET IDAMBIENTE = @IdAmbTrabj,
				[COD] = (SELECT [COD] FROM [Conf].[Tpo_Amb_Trabj] WHERE [IDAMBIENTE] = @IdAmbTrabj),
				[DESCRIPCION] = (SELECT [DESCRIPCION] FROM [Conf].[Tpo_Amb_Trabj] WHERE [IDAMBIENTE] = @IdAmbTrabj)
		WHERE [RUCENTITY] = @RUCENTITY
	END



--	IF NOT EXISTS (SELECT ID FROM [Conf].[AmbienteTrabj] WHERE RUCENTITY = @RUCENTITY)
--	BEGIN
--		INSERT INTO [Conf].[AmbienteTrabj] 
--			(IDAMBIENTE, COD, DESCRIPCION, RUCENTITY)
--		VALUES 
--			(@IdAmbTrabj, (SELECT [COD] FROM [Conf].[Tpo_Amb_Trabj] WHERE [IDAMBIENTE] = @IdAmbTrabj)
--			,(SELECT [DESCRIPCION] FROM [Conf].[Tpo_Amb_Trabj] WHERE [IDAMBIENTE] = @IdAmbTrabj)
--			,@RUCENTITY)
--	END
--ELSE
--	BEGIN
--		UPDATE [Conf].[AmbienteTrabj]
--			SET IDAMBIENTE = @IdAmbTrabj,
--				[COD] = (SELECT [COD] FROM [Conf].[Tpo_Amb_Trabj] WHERE [IDAMBIENTE] = @IdAmbTrabj),
--				[DESCRIPCION] = (SELECT [DESCRIPCION] FROM [Conf].[Tpo_Amb_Trabj] WHERE [IDAMBIENTE] = @IdAmbTrabj)
--		WHERE [RUCENTITY] = @RUCENTITY
--	END
GO
/****** Object:  StoredProcedure [Seg].[Usp_UpdateEmpresaBs]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_UpdateEmpresaBs]
	@IdEmpresa [int],
	@RucEmpresa [varchar](11),
	@RazonSocial [varchar](250),
	@Direccion [varchar](250),
	@Telefono [varchar](20),
	@Email [varchar](250),
	@Password [varchar](250)
WITH EXECUTE AS CALLER
AS
BEGIN
	UPDATE [Mtro].[Empresa]
		SET [RazonSocial] = @RazonSocial,
			[Direccion] = @Direccion,
			[Telefono] = @Telefono,
			[Email] = @Email,
			[Password] = @Password
	WHERE [RucEmpresa] = @RucEmpresa AND
		  [IdEmpresa] = @IdEmpresa
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_UpdateRegistroLogueo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_UpdateRegistroLogueo]
@ID INT,
@DNI_USUARIO VARCHAR(11),
@USERNAME VARCHAR(50),
@FECHASALIDA DATETIME
AS
BEGIN
	UPDATE [Seg].[RegistroLogueo]
		SET FECHASALIDA = @FECHASALIDA
	WHERE ID = @ID AND DNI_USUARIO = @DNI_USUARIO AND USERNAME = @USERNAME
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ValidarDniUsuario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ValidarDniUsuario]
	@Dni_Ruc [varchar](30)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		[IdUsuario]
		,[Nombres]
		,[DNI_RUC] AS DNI
	FROM [Seg].[Usuarios]
	WHERE [DNI_RUC] = @Dni_Ruc
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ValidarExistsCorreoEmpresa]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ValidarExistsCorreoEmpresa]
	@IdEmpresa [int],
	@Email [varchar](150),
	@RucEmpresa [varchar](11),
	@TypeMail [varchar](10)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 		
		SC.IdEmail,
		--CONVERT(INT, ROW_NUMBER() OVER(ORDER BY SC.IdEmail)) AS IdEmail,
		ME.IdEmpresa,
		ME.RazonSocial,
		SC.Email
		--SC.[Password] AS 'Password'
	FROM [Seg].[Correo] AS SC INNER JOIN
		 [Mtro].[Empresa] AS ME ON SC.IdEmpresa = ME.IdEmpresa
	WHERE (ME.IdEmpresa = @IdEmpresa) AND
		  (ME.Ruc = @RucEmpresa) AND
		  (SC.Email = @Email) AND
		  (TypeMail) = @TypeMail
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ValidarUsername]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Seg].[Usp_ValidarUsername]
	@Username [varchar](250)
WITH EXECUTE AS CALLER
AS
BEGIN
	SELECT 
		[Username]
		,[DNI_RUC] AS DNI
	FROM [Seg].[Usuarios]
	WHERE [Username] = @Username
END


GO
/****** Object:  UserDefinedFunction [Fact].[fn_GetCodTpoDoc]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [Fact].[fn_GetCodTpoDoc](@idtipodoc int)
RETURNS [varchar](2) WITH EXECUTE AS CALLER
AS 
begin
	declare @codtipodoc varchar(2)
	set @codtipodoc = (select Code
	 FROM [Ctl].[TipoDocumento] 
	WHERE Id_TD = @idtipodoc)
	return @codtipodoc
end
GO
/****** Object:  UserDefinedFunction [Fact].[fn_GetIdCE]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Fact].[fn_GetIdCE](@NUM_CPE varchar(50))
RETURNS [int] WITH EXECUTE AS CALLER
AS
begin
	declare @idce int
	set @idce = (
      select Id_DC
      FROM Fact.[O.DocumentoCabecera] 
      WHERE NUM_CPE = @NUM_CPE
         )
	return @idce
end

GO
/****** Object:  UserDefinedFunction [Fact].[fn_GetNroEnv]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Fact].[fn_GetNroEnv]()
RETURNS [int] WITH EXECUTE AS CALLER
AS 
begin
	declare @nroenv int
	set @nroenv = (
      select MaxNumAttempts
      FROM Conf.TimeService 
      WHERE CodeService = 'ADE.Services.SunatDelivery'
         )
	return @nroenv
end

GO
/****** Object:  UserDefinedFunction [Mtro].[fn_GetCodPais]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Mtro].[fn_GetCodPais](@codigoubigeo [varchar](6))
RETURNS [varchar](200) WITH EXECUTE AS CALLER
AS 
begin
	declare @codpais varchar(200)
	set @codpais = (select MDT.[Descripcion]
	 FROM [Mtro].[Pais] AS MPS INNER JOIN 
		  [Mtro].[Departamento] AS MD ON MPS.Id = MD.IdPais INNER JOIN
		  [Mtro].[Provincia] AS MP ON MD.Id = MP.IdDepartamento INNER JOIN
		  [Mtro].[Distrito] AS MDT ON MP.Id = MDT.IdProvincia
	WHERE MDT.CodigoUbigeo = @codigoubigeo)
	return @codpais
end

GO
/****** Object:  UserDefinedFunction [Mtro].[fn_GetDepartamento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Mtro].[fn_GetDepartamento](@codigoubigeo [varchar](6))
RETURNS [varchar](200) WITH EXECUTE AS CALLER
AS 
begin
	declare @departamento varchar(200)
	set @departamento = (select MDT.[Descripcion]
	 FROM [Mtro].[Pais] AS MPS INNER JOIN 
		  [Mtro].[Departamento] AS MD ON MPS.Id = MD.IdPais INNER JOIN
		  [Mtro].[Provincia] AS MP ON MD.Id = MP.IdDepartamento INNER JOIN
		  [Mtro].[Distrito] AS MDT ON MP.Id = MDT.IdProvincia
	WHERE MDT.CodigoUbigeo = @codigoubigeo)
	return @departamento
end

GO
/****** Object:  UserDefinedFunction [Mtro].[fn_GetDireccionEmpresaBs]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Mtro].[fn_GetDireccionEmpresaBs](@IdEmpresa [varchar](11))
RETURNS [varchar](250) WITH EXECUTE AS CALLER
AS 
begin
	declare @direccion varchar(250)
	set @direccion = (select [Direccion]
	 FROM [Mtro].[Empresa]
	WHERE [IdEmpresa] = @IdEmpresa)
	return @direccion
end

GO
/****** Object:  UserDefinedFunction [Mtro].[fn_GetDistrito]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Mtro].[fn_GetDistrito](@codigoubigeo [varchar](6))
RETURNS [varchar](200) WITH EXECUTE AS CALLER
AS 
begin
	declare @distrito varchar(200)
	set @distrito = (select MDT.[Descripcion]
	 FROM [Mtro].[Pais] AS MPS INNER JOIN 
		  [Mtro].[Departamento] AS MD ON MPS.Id = MD.IdPais INNER JOIN
		  [Mtro].[Provincia] AS MP ON MD.Id = MP.IdDepartamento INNER JOIN
		  [Mtro].[Distrito] AS MDT ON MP.Id = MDT.IdProvincia
	WHERE MDT.CodigoUbigeo = @codigoubigeo)
	return @distrito
end

GO
/****** Object:  UserDefinedFunction [Mtro].[fn_GetProvincia]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Mtro].[fn_GetProvincia](@codigoubigeo [varchar](6))
RETURNS [varchar](200) WITH EXECUTE AS CALLER
AS 
begin
	declare @provincia varchar(200)
	set @provincia = (select MDT.[Descripcion]
	 FROM [Mtro].[Pais] AS MPS INNER JOIN 
		  [Mtro].[Departamento] AS MD ON MPS.Id = MD.IdPais INNER JOIN
		  [Mtro].[Provincia] AS MP ON MD.Id = MP.IdDepartamento INNER JOIN
		  [Mtro].[Distrito] AS MDT ON MP.Id = MDT.IdProvincia
	WHERE MDT.CodigoUbigeo = @codigoubigeo)
	return @provincia
end

GO
/****** Object:  UserDefinedFunction [Mtro].[fn_GetRazonSocialEmpresaBs]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Mtro].[fn_GetRazonSocialEmpresaBs](@IdEmpresa [varchar](11))
RETURNS [varchar](250) WITH EXECUTE AS CALLER
AS 
begin
	declare @razonsocial varchar(250)
	set @razonsocial = (select [RazonSocial]
	 FROM [Mtro].[Empresa]
	WHERE [IdEmpresa] = @IdEmpresa)
	return @razonsocial
end

GO
/****** Object:  UserDefinedFunction [Mtro].[fn_GetRucEmpresaBs]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Mtro].[fn_GetRucEmpresaBs](@IdEmpresa [varchar](11))
RETURNS [varchar](11) WITH EXECUTE AS CALLER
AS 
begin
	declare @rucempresa varchar(200)
	set @rucempresa = (select [Ruc]
	 FROM [Mtro].[Empresa]
	WHERE [IdEmpresa] = @IdEmpresa)
	return @rucempresa
end

GO
/****** Object:  UserDefinedFunction [Mtro].[fn_GetTelefonoEmpresaBs]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [Mtro].[fn_GetTelefonoEmpresaBs](@IdEmpresa [varchar](11))
RETURNS [varchar](20) WITH EXECUTE AS CALLER
AS 
begin
	declare @telefono varchar(20)
	set @telefono = (select [Telefono]
	 FROM [Mtro].[Empresa]
	WHERE [IdEmpresa] = @IdEmpresa)
	return @telefono
end

GO
/****** Object:  Table [Conf].[AmbienteTrabj]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Conf].[AmbienteTrabj](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[COD] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[DESCRIPCION] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[IDAMBIENTE] [int] NULL,
	[RUCENTITY] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Ambiente__3214EC27671EF3FD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Conf].[CertificateDigital]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Conf].[CertificateDigital](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameCertificate] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[Pwd] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[ExpirationDate] [varchar](10) COLLATE Modern_Spanish_CI_AS NULL,
	[RucEntity] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Conf].[DocumentoAmbiente]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Conf].[DocumentoAmbiente](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_TIPO_CE] [int] NULL,
	[TIPO_CE] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[DESCR_TPO_CE] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[IDAMBIENTE] [int] NULL,
	[IDESTADO] [int] NULL,
	[RUCENTITY] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Document__3214EC27E55D6ABF] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Conf].[MailNotifications]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Conf].[MailNotifications](
	[RucEntity] [varchar](11) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Correos] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[ContactName] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[TypeMail] [varchar](1) COLLATE Modern_Spanish_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Conf].[TimeService]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Conf].[TimeService](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CodeService] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[NameService] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[ValueTime] [varchar](5) COLLATE Modern_Spanish_CI_AS NULL,
	[IntervalValue] [varchar](5) COLLATE Modern_Spanish_CI_AS NULL,
	[MaxNumAttempts] [int] NULL,
	[RucEntity] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
	[SubType] [varchar](5) COLLATE Modern_Spanish_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Conf].[Tpo_Amb_Trabj]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Conf].[Tpo_Amb_Trabj](
	[IDAMBIENTE] [int] IDENTITY(1,1) NOT NULL,
	[COD] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[DESCRIPCION] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[IDESTADO] [int] NULL,
 CONSTRAINT [PK__Tpo_Amb___48397635D31A36AC] PRIMARY KEY CLUSTERED 
(
	[IDAMBIENTE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Conf].[URL_SUNAT]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Conf].[URL_SUNAT](
	[ID_URL] [int] IDENTITY(1,1) NOT NULL,
	[COD_URL] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[DESCR_URL] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[URL] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[IDESTADO] [int] NULL,
	[IDAMBIENTE] [int] NULL,
	[NAME] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[DOCUMENTOS] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__URL_SUNA__2A8F9FFC452F4613] PRIMARY KEY CLUSTERED 
(
	[ID_URL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[CalculoISC]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[CalculoISC](
	[Id_ISC] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Desc] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Obsv] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
 CONSTRAINT [PK__CalculoI__5FB7321DC76DA45C] PRIMARY KEY CLUSTERED 
(
	[Id_ISC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[ConceptosTributarios]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[ConceptosTributarios](
	[Id_CT] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Desc] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Obsv] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
 CONSTRAINT [PK__Concepto__16EC97F56852065E] PRIMARY KEY CLUSTERED 
(
	[Id_CT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[DocumentoTributario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[DocumentoTributario](
	[Id_DT] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Desc] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Obsv] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
 CONSTRAINT [PK__Document__16EC9FD43A379B85] PRIMARY KEY CLUSTERED 
(
	[Id_DT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[ElementoFacturaBoleta]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[ElementoFacturaBoleta](
	[Id_EFB] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Descr] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Obsv] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
 CONSTRAINT [PK__Elemento__5E614689447F3021] PRIMARY KEY CLUSTERED 
(
	[Id_EFB] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[ErroresRespuestaSunat]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[ErroresRespuestaSunat](
	[ERR_COD] [varchar](6) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[ERR_DES] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK_ErroresRespuestaSunat] PRIMARY KEY CLUSTERED 
(
	[ERR_COD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[TipoAfectacionIGV]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[TipoAfectacionIGV](
	[Id_TAIGV] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Desc] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Obsv] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
 CONSTRAINT [PK__TipoAfec__677D01AA2E57A025] PRIMARY KEY CLUSTERED 
(
	[Id_TAIGV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[TipoDocumento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[TipoDocumento](
	[Id_TD] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[Desc] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Un1001] [varchar](10) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
	[Padre] [int] NULL,
 CONSTRAINT [PK__TipoDocu__16EC19C8AC8DCBD0] PRIMARY KEY CLUSTERED 
(
	[Id_TD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[TipoDocumentoIdentidad]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[TipoDocumentoIdentidad](
	[Id_TDI] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Desc] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Obsv] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
	[Descripcion] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__TipoDocu__52E09E13525D3AEA] PRIMARY KEY CLUSTERED 
(
	[Id_TDI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[TipoNotaCredito]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[TipoNotaCredito](
	[Id_NC] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Desc] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Obsv] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
 CONSTRAINT [PK__TipoNota__16ECEE9FB93C2F4D] PRIMARY KEY CLUSTERED 
(
	[Id_NC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[TipoNotaDebito]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[TipoNotaDebito](
	[Id_ND] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Desc] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Obsv] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
 CONSTRAINT [PK__TipoNota__16ECEE8067B78D40] PRIMARY KEY CLUSTERED 
(
	[Id_ND] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[TipoPrecioVentaUnitario]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[TipoPrecioVentaUnitario](
	[Id_TPV] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Desc] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Obsv] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
 CONSTRAINT [PK__TipoPrec__52E07EA35327FEDF] PRIMARY KEY CLUSTERED 
(
	[Id_TPV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[TipoTributos]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[TipoTributos](
	[Id_TT] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Desc] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[UnEce5153] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[Obsv] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
 CONSTRAINT [PK__TipoTrib__16EC1838C9E6839C] PRIMARY KEY CLUSTERED 
(
	[Id_TT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ctl].[TipoValorVenta]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ctl].[TipoValorVenta](
	[Id_VV] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Desc] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Obsv] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
 CONSTRAINT [PK__TipoValo__16EC29FCB0FC84F5] PRIMARY KEY CLUSTERED 
(
	[Id_VV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[Coin]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[Coin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Simbolo] [varchar](5) COLLATE Modern_Spanish_CI_AS NULL,
	[Descripcion] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[CodeInternal] [varchar](10) COLLATE Modern_Spanish_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[CONFIG_MAIN]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[CONFIG_MAIN](
	[TAB] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[NOM] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[POS] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[VAL] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[MND] [varchar](1) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[MSG] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[ECV] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[ECN] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[CredentialsCertificateAmb]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[CredentialsCertificateAmb](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RucEntity] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[NombreUsuario] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Password] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[IDAMBIENTE] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[DocumentoEnviado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[DocumentoEnviado](
	[IdDocEnviado] [int] IDENTITY(1,1) NOT NULL,
	[IdTipoDocumento] [int] NULL,
	[Serie] [varchar](4) COLLATE Modern_Spanish_CI_AS NULL,
	[Correlativo] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[Destino] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Asunto] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Mensaje] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Remitente] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[FechaEnvio] [datetime] NULL,
	[Fecha_Cad] [varchar](25) COLLATE Modern_Spanish_CI_AS NULL,
	[RucEmpresa] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[TPO_CE] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Document__72239FCB28EB3329] PRIMARY KEY CLUSTERED 
(
	[IdDocEnviado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[ExchangeRate]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[ExchangeRate](
	[RucNumber] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[Fecha] [datetime] NULL,
	[Moneda] [varchar](5) COLLATE Modern_Spanish_CI_AS NULL,
	[Cambio] [decimal](10, 5) NULL,
	[fech_str] [varchar](10) COLLATE Modern_Spanish_CI_AS NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[I.Bajadocumento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [Fact].[I.Bajadocumento](
	[Id_Bajadocu] [int] IDENTITY(1,1) NOT NULL,
	[Tipodocu] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[Serie] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[Folio] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[Fechabaja] [datetime] NULL,
	[MotivoAnulacion] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Estadobaja] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[Mensajebaja] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Correlativo] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[Estadoenvio] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[Mensajeenvio] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Numatencion] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK_In_Bajadocumento] PRIMARY KEY CLUSTERED 
(
	[Id_Bajadocu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[I.detalleDocumento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [Fact].[I.detalleDocumento](
	[Id_Detalledoc] [int] IDENTITY(1,1) NOT NULL,
	[Correlativo] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[Tipodocu] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[Serie] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[Fechacarga] [datetime] NULL,
	[Motivoanulacion] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Mensajedoc] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Id_Bajadocu] [int] NULL,
 CONSTRAINT [PK_In_Detalledocumento] PRIMARY KEY CLUSTERED 
(
	[Id_Detalledoc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[I.DocumentoCabecera]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [Fact].[I.DocumentoCabecera](
	[Id_DC] [int] IDENTITY(1,1) NOT NULL,
	[NUM_CPE] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[ID_TPO_CPE] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[ID_CPE] [varchar](13) COLLATE Modern_Spanish_CI_AS NULL,
	[ID_TPO_OPERACION] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[FEC_EMIS] [datetime] NULL,
	[TPO_MONEDA] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[TPO_NOTA] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[MOTIVO_NOTA] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_TPO_DOC] [varchar](1) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_NUM_DOCU] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_NOMBRE] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_NCOMER] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_UBIGEO] [varchar](6) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_DFISCAL] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_DURBANIZ] [varchar](25) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_DIR_PROV] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_DIR_DPTO] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_DIR_DIST] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_COD_PAIS] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_TPODOC] [varchar](1) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_NUMDOC] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_NOMBRE] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_DIRECCION] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[TOT_GRAV_MTO] [decimal](12, 2) NULL,
	[TOT_INAF_MTO] [decimal](12, 2) NULL,
	[TOT_EXON_MTO] [decimal](12, 2) NULL,
	[TOT_GRAT_MTO] [decimal](15, 2) NULL,
	[TOT_DSCTO_MTO] [decimal](12, 2) NULL,
	[TOT_SUMA_IGV] [decimal](12, 2) NULL,
	[TOT_SUMA_ISC] [decimal](12, 2) NULL,
	[TOT_SUMA_OTRIB] [decimal](12, 2) NULL,
	[TOT_DCTO_GLOB] [decimal](12, 2) NULL,
	[TOT_SUM_OCARG] [decimal](12, 2) NULL,
	[ANT_TOT_ANTICIPO] [decimal](12, 2) NULL,
	[TOT_IMPOR_TOTAL] [decimal](12, 2) NULL,
	[MONTOLITERAL] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[PER_BASE_IMPO] [decimal](12, 2) NULL,
	[PER_MNTO_PER] [decimal](12, 2) NULL,
	[PER_MNTO_TOT] [decimal](12, 2) NULL,
	[SERIE] [varchar](4) COLLATE Modern_Spanish_CI_AS NULL,
	[NUM_DOCUMENTO] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[Id_ED] [int] NULL,
	[CodigoPDF417] [image] NULL,
	[VAR_FIR] [varbinary](max) NULL,
	[VAR_RES] [varbinary](max) NULL,
	[DOC_EST] [varchar](5) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_COD] [varchar](5) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_MSG] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Id_ED_DOC] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_DC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[I.DocumentoDetalle]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [Fact].[I.DocumentoDetalle](
	[Id_DD] [int] IDENTITY(1,1) NOT NULL,
	[Id_DC] [int] NULL,
	[NUM_CPE] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[IT_NRO_ORD] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[IT_UND_MED] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[IT_CANT_ITEM] [numeric](18, 5) NULL,
	[IT_COD_PROD] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[IT_DESCRIP] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[IT_VAL_UNIT] [decimal](18, 2) NULL,
	[IT_MNT_PVTA] [decimal](18, 2) NULL,
	[IT_VAL_VTA] [decimal](18, 2) NULL,
	[IT_MTO_IGV] [decimal](18, 2) NULL,
	[IT_COD_AFE_IGV] [varchar](4) COLLATE Modern_Spanish_CI_AS NULL,
	[IT_MTO_ISC] [decimal](18, 2) NULL,
	[IT_SIS_AFE_ISC] [decimal](18, 2) NULL,
	[IT_DESC_MNTO] [decimal](18, 2) NULL,
	[SERIE] [varchar](4) COLLATE Modern_Spanish_CI_AS NULL,
	[NUM_DOCUMENTO] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL
) ON [PRIMARY]
SET ANSI_PADDING ON
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [TPODOCRELAC] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [NUMDOCRELAC] [varchar](13) COLLATE Modern_Spanish_CI_AS NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [FEMISDOCRELAC] [datetime] NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [ITOTDOCRELAC] [decimal](12, 2) NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [MDOCRELAC] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [FECMOVI] [datetime] NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [NUMMOVI] [varchar](9) COLLATE Modern_Spanish_CI_AS NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [IMPSOPERMOV] [decimal](12, 2) NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [MONMOVI] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [IMPOPER] [decimal](12, 2) NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [MONIMPOPER] [varchar](13) COLLATE Modern_Spanish_CI_AS NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [FECOPER] [datetime] NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [IMPTOTOPER] [decimal](12, 2) NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [MONOPER] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [MONREFETC] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [MONDESTTC] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [FACTORTC] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL
ALTER TABLE [Fact].[I.DocumentoDetalle] ADD [FECHATC] [datetime] NULL
 CONSTRAINT [PK__I.Docume__16EC9FC4300E1035] PRIMARY KEY CLUSTERED 
(
	[Id_DD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.Afectado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.Afectado](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_DC] [int] NULL,
	[NRO_ORD] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_AFEC_ID] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[ID_TPO_CPE] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__O.Afecta__3214EC276DF0445C] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.Anticipo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.Anticipo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_DC] [int] NULL,
	[ANT_NROORDEN] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[ANT_MONTO] [decimal](12, 2) NULL,
	[ANT_TPO_DOC_ANT] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[ANT_ID_DOC_ANT] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[ANT_NUM_DOC_EMI] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[ANT_TPO_DOC_EMI] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__O.Antici__3214EC275E363492] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.Detracciones]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.Detracciones](
	[ID_DE] [int] IDENTITY(1,1) NOT NULL,
	[ID_DC] [int] NULL,
	[NUM_CPE] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[VAL_BBSS] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[CTA_BN] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[PORCENT] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[MONTO] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.Documento_Anul]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.Documento_Anul](
	[Id_DocAnul] [int] IDENTITY(1,1) NOT NULL,
	[Id_TpoDoc] [int] NULL,
	[Serie] [varchar](4) COLLATE Modern_Spanish_CI_AS NULL,
	[Correlativo] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[Fecha_Anul] [datetime] NULL,
	[Motivo_Anul] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Estado_Anul] [int] NULL,
	[Mensaje_Anul] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[MensajeEnvio] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[NumAtencion] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Estado_Envio] [int] NULL,
	[RucEmpresa] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[TypeDoc] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[NUM_CE] [varchar](40) COLLATE Modern_Spanish_CI_AS NULL,
	[Usuario] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Id_ED] [int] NULL,
 CONSTRAINT [PK__O.Docume__57A67015EB897FCC] PRIMARY KEY CLUSTERED 
(
	[Id_DocAnul] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.DocumentoAfectado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.DocumentoAfectado](
	[Id_DA] [int] IDENTITY(1,1) NOT NULL,
	[Id_DC] [int] NULL,
	[DOC_AFEC_ID] [varchar](13) COLLATE Modern_Spanish_CI_AS NULL,
	[ID_TPO_CPE] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[NRO_ORD] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__O.Docume__16EC9FC157DD13AD] PRIMARY KEY CLUSTERED 
(
	[Id_DA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.DocumentoCabecera]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.DocumentoCabecera](
	[Id_DC] [int] IDENTITY(1,1) NOT NULL,
	[NUM_CPE] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[ID_TPO_CPE] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[ID_CPE] [varchar](13) COLLATE Modern_Spanish_CI_AS NULL,
	[ID_TPO_OPERACION] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[FEC_EMIS] [datetime] NULL,
	[TPO_MONEDA] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[TPO_NOTA] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[MOTIVO_NOTA] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_TPO_DOC] [varchar](1) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_NUM_DOCU] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_NOMBRE] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_NCOMER] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_UBIGEO] [varchar](6) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_DFISCAL] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_DURBANIZ] [varchar](25) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_DIR_PROV] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_DIR_DPTO] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_DIR_DIST] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[EM_COD_PAIS] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_TPODOC] [varchar](1) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_NUMDOC] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_NOMBRE] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_DIRECCION] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[TOT_GRAV_MTO] [decimal](12, 2) NULL,
	[TOT_INAF_MTO] [decimal](12, 2) NULL,
	[TOT_EXON_MTO] [decimal](12, 2) NULL,
	[TOT_GRAT_MTO] [decimal](15, 2) NULL,
	[TOT_DSCTO_MTO] [decimal](12, 2) NULL,
	[TOT_SUMA_IGV] [decimal](12, 2) NULL,
	[TOT_SUMA_ISC] [decimal](12, 2) NULL,
	[TOT_SUMA_OTRIB] [decimal](12, 2) NULL,
	[TOT_DCTO_GLOB] [decimal](12, 2) NULL,
	[TOT_SUM_OCARG] [decimal](12, 2) NULL,
	[ANT_TOT_ANTICIPO] [decimal](15, 2) NULL,
	[TOT_IMPOR_TOTAL] [decimal](12, 2) NULL,
	[MONTOLITERAL] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[PER_BASE_IMPO] [decimal](12, 2) NULL,
	[PER_MNTO_PER] [decimal](12, 2) NULL,
	[PER_MNTO_TOT] [decimal](12, 2) NULL,
	[SERIE] [varchar](4) COLLATE Modern_Spanish_CI_AS NULL,
	[NUM_DOCUMENTO] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[Id_ED] [int] NULL,
	[CodigoPDF417] [image] NULL,
	[VAR_FIR] [varbinary](max) NULL,
	[VAR_RES] [varbinary](max) NULL,
	[DOC_EST] [varchar](5) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_COD] [varchar](5) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_MSG] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Id_ED_DOC] [int] NULL,
	[LogoEmpresa] [image] NULL,
	[RE_NCOMER] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_DURBANIZ] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_DIR_PROV] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_DIR_DPTO] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_DIR_DIST] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_COD_PAIS] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[RE_UBIGEO] [varchar](6) COLLATE Modern_Spanish_CI_AS NULL,
	[REGIMENCE] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[TASACE] [decimal](12, 2) NULL,
	[OBSCE] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[IMPTOTCE] [decimal](12, 2) NULL,
	[MONIMPTOTCE] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[IMPTOT] [decimal](12, 2) NULL,
	[MONIMPTOT] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[SEDE] [varchar](255) COLLATE Modern_Spanish_CI_AS NULL,
	[USUARIO] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
	[IMPRESORA] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
	[CAMPO1] [varchar](255) COLLATE Modern_Spanish_CI_AS NULL,
	[CAMPO2] [varchar](255) COLLATE Modern_Spanish_CI_AS NULL,
	[CAMPO3] [varchar](255) COLLATE Modern_Spanish_CI_AS NULL,
	[CAMPO4] [varchar](255) COLLATE Modern_Spanish_CI_AS NULL,
	[CAMPO5] [varchar](255) COLLATE Modern_Spanish_CI_AS NULL,
	[CAMPO6] [varchar](255) COLLATE Modern_Spanish_CI_AS NULL,
	[CAMPO7] [varchar](255) COLLATE Modern_Spanish_CI_AS NULL,
	[CAMPO8] [varchar](255) COLLATE Modern_Spanish_CI_AS NULL,
	[CAMPO9] [varchar](255) COLLATE Modern_Spanish_CI_AS NULL,
	[CAMPO10] [varchar](255) COLLATE Modern_Spanish_CI_AS NULL,
	[StatuSend] [int] NULL,
	[StatusPrint] [int] NULL,
	[REF_FILES] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[SYS_DATE] [datetime] NULL,
	[CDR_DATE] [datetime] NULL,
	[SUMMARY] [int] NULL,
	[NRO_ENV] [int] NULL,
	[TypeFormat] [int] NULL,
	[VOIDED] [int] NULL,
 CONSTRAINT [PK__O.DocumentoCabec__0A688BB1] PRIMARY KEY CLUSTERED 
(
	[Id_DC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.DocumentoDetalle]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.DocumentoDetalle](
	[Id_DD] [int] IDENTITY(1,1) NOT NULL,
	[Id_DC] [int] NULL,
	[NUM_CPE] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[IT_NRO_ORD] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[IT_UND_MED] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[IT_CANT_ITEM] [numeric](18, 5) NULL,
	[IT_COD_PROD] [char](30) COLLATE Modern_Spanish_CI_AS NULL,
	[IT_DESCRIP] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[IT_VAL_UNIT] [decimal](18, 5) NULL,
	[IT_MNT_PVTA] [decimal](18, 5) NULL,
	[IT_VAL_VTA] [decimal](18, 2) NULL,
	[IT_MTO_IGV] [decimal](18, 2) NULL,
	[IT_COD_AFE_IGV] [varchar](4) COLLATE Modern_Spanish_CI_AS NULL,
	[IT_MTO_ISC] [decimal](18, 2) NULL,
	[IT_SIS_AFE_ISC] [decimal](18, 2) NULL,
	[IT_DESC_MNTO] [decimal](18, 2) NULL,
	[SERIE] [varchar](4) COLLATE Modern_Spanish_CI_AS NULL,
	[NUM_DOCUMENTO] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[TPODOCRELAC] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[NUMDOCRELAC] [varchar](13) COLLATE Modern_Spanish_CI_AS NULL,
	[FEMISDOCRELAC] [datetime] NULL,
	[ITOTDOCRELAC] [decimal](12, 2) NULL,
	[MDOCRELAC] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[FECMOVI] [datetime] NULL,
	[NUMMOVI] [varchar](9) COLLATE Modern_Spanish_CI_AS NULL,
	[IMPSOPERMOV] [decimal](12, 2) NULL,
	[MONMOVI] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[IMPOPER] [decimal](12, 2) NULL,
	[MONIMPOPER] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[FECOPER] [datetime] NULL,
	[IMPTOTOPER] [decimal](12, 2) NULL,
	[MONOPER] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[MONREFETC] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[MONDESTTC] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[FACTORTC] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[FECHATC] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_DD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.EstadoDoc]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.EstadoDoc](
	[Id_ED_DOC] [int] IDENTITY(1,1) NOT NULL,
	[Desc] [varchar](40) COLLATE Modern_Spanish_CI_AS NULL,
	[Abrev] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[Color] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[DescGeneral] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
	[RutaImagen] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__O.Estado__DD1C3A89E9B3DA95] PRIMARY KEY CLUSTERED 
(
	[Id_ED_DOC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.EstadoDocumento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.EstadoDocumento](
	[Id_ED] [int] IDENTITY(1,1) NOT NULL,
	[Desc] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[Abrev] [varchar](5) COLLATE Modern_Spanish_CI_AS NULL,
	[Color] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[DescGeneral] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
	[RutaImagen] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__O.Estado__16ECA7A7749A02AC] PRIMARY KEY CLUSTERED 
(
	[Id_ED] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.Extra]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.Extra](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_DC] [int] NULL,
	[EXLINEA] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[EXDATO] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[EXTIPO] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__O.Extra__3214EC279F154AB1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.FlexMailEnvio]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.FlexMailEnvio](
	[ID_CE] [int] IDENTITY(1,1) NOT NULL,
	[ID_DC] [int] NULL,
	[NUM_CPE] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[PARA] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[CC] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[CCO] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__O.FlexMa__8B622F8EBB5B137F] PRIMARY KEY CLUSTERED 
(
	[ID_CE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.NotasRespuesta]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.NotasRespuesta](
	[NUM_CPE] [varchar](50) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[ID_CPE] [int] NULL,
	[ERR_COD] [varchar](6) COLLATE Modern_Spanish_CI_AS NULL,
	[ERR_TXT] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
	[TIPO] [char](1) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK_O.NotasRespuesta] PRIMARY KEY CLUSTERED 
(
	[NUM_CPE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.RBajasCabecera]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.RBajasCabecera](
	[ID_RAC] [int] IDENTITY(1,1) NOT NULL,
	[NUM_CPE] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[TOT_DOC] [int] NULL,
	[FEC_REF] [datetime] NULL,
	[FEC_ANU] [datetime] NULL,
	[FEC_ENV] [datetime] NULL,
	[DOC_EST] [varchar](10) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_MSG] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_TCK] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[NUM_SEC] [int] NULL,
	[FEC_CAD] [varchar](25) COLLATE Modern_Spanish_CI_AS NULL,
	[VAR_FIR] [varbinary](max) NULL,
	[VAR_RES] [varbinary](max) NULL,
	[TIPO] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[RucEntity] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[SYS_DATE] [datetime] NULL,
	[CDR_DATE] [datetime] NULL,
	[NRO_ENV] [int] NULL,
 CONSTRAINT [PK_O.RAnulacionesCabecera] PRIMARY KEY CLUSTERED 
(
	[ID_RAC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.RBajasDetalle]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.RBajasDetalle](
	[ID_RAD] [int] IDENTITY(1,1) NOT NULL,
	[NUM_CPE] [char](50) COLLATE Modern_Spanish_CI_AS NULL,
	[TPO_CPE] [char](4) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_SER] [char](6) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_NUM] [varchar](10) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_DES] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_FEC] [varchar](10) COLLATE Modern_Spanish_CI_AS NULL,
	[ID_RAC] [int] NULL,
 CONSTRAINT [PK_O.RAnulacionesDetalle] PRIMARY KEY CLUSTERED 
(
	[ID_RAD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.RBoletasCabecera]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.RBoletasCabecera](
	[ID_RBC] [int] IDENTITY(1,1) NOT NULL,
	[NUM_CPE] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[TOT_DOC] [int] NULL,
	[FEC_INI] [datetime] NULL,
	[FEC_FIN] [datetime] NULL,
	[FEC_ENV] [datetime] NULL,
	[MTO_GRA] [decimal](15, 2) NULL,
	[MTO_EXO] [decimal](15, 2) NULL,
	[MTO_INA] [decimal](15, 2) NULL,
	[MTO_OCA] [decimal](15, 2) NULL,
	[IMP_IGV] [decimal](15, 2) NULL,
	[IMP_ISC] [decimal](15, 2) NULL,
	[IMP_OTH] [decimal](15, 2) NULL,
	[MTO_TOT] [decimal](15, 2) NULL,
	[DOC_EST] [varchar](10) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_MSG] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_TCK] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[NUM_SEC] [int] NULL,
	[FEC_CAD] [varchar](25) COLLATE Modern_Spanish_CI_AS NULL,
	[VAR_FIR] [varbinary](max) NULL,
	[VAR_RES] [varbinary](max) NULL,
	[RucEntity] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[SYS_DATE] [datetime] NULL,
	[CDR_DATE] [datetime] NULL,
	[NRO_ENV] [int] NULL,
 CONSTRAINT [PK_O.RBoletasCabecera] PRIMARY KEY CLUSTERED 
(
	[ID_RBC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.RBoletasDetalle]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.RBoletasDetalle](
	[ID_RBD] [int] IDENTITY(1,1) NOT NULL,
	[NUM_CPE] [char](50) COLLATE Modern_Spanish_CI_AS NULL,
	[TPO_CPE] [char](4) COLLATE Modern_Spanish_CI_AS NULL,
	[DOC_SER] [char](6) COLLATE Modern_Spanish_CI_AS NULL,
	[NUM_INI] [int] NULL,
	[NUM_FIN] [int] NULL,
	[MTO_GRA] [decimal](15, 2) NULL,
	[MTO_EXO] [decimal](15, 2) NULL,
	[MTO_INA] [decimal](15, 2) NULL,
	[MTO_OCA] [decimal](15, 2) NULL,
	[IMP_IGV] [decimal](15, 2) NULL,
	[IMP_ISC] [decimal](15, 2) NULL,
	[IMP_OTH] [decimal](15, 2) NULL,
	[MTO_TOT] [decimal](15, 2) NULL,
	[ID_RBC] [int] NULL,
	[NRO_LIN] [int] NULL,
 CONSTRAINT [PK_O.RBoletasDetalle] PRIMARY KEY CLUSTERED 
(
	[ID_RBD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.Referencia]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.Referencia](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_DC] [int] NULL,
	[REF_NROORDEN] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
	[REF_ID] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[REF_TPO_DOC] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__O.Refere__3214EC274E7E14FF] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[O.Resu_DocAnul]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Fact].[O.Resu_DocAnul](
	[Id_RDA] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime] NULL,
 CONSTRAINT [PK__O.Resu_D__55907C0BAF568D12] PRIMARY KEY CLUSTERED 
(
	[Id_RDA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Fact].[O.Serie]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[O.Serie](
	[Id_S] [int] IDENTITY(1,1) NOT NULL,
	[Id_TD] [int] NULL,
	[CodeDoc] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[NumSerie] [char](7) COLLATE Modern_Spanish_CI_AS NULL,
	[Obsv] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
	[IdEmpresa] [int] NULL,
 CONSTRAINT [PK__O.Serie__B770B53B6C4DF78E] PRIMARY KEY CLUSTERED 
(
	[Id_S] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[TipoDocumentPrint]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[TipoDocumentPrint](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TIPO_CE] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[IDESTADO] [int] NULL,
	[RucEntity] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Fact].[TipoDocumentSend]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Fact].[TipoDocumentSend](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TIPO_CE] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[IDESTADO] [int] NULL,
	[RucEntity] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Mtro].[Cliente]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Mtro].[Cliente](
	[IdCliente] [int] IDENTITY(1,1) NOT NULL,
	[RazonSocial] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[NroDocumento] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[Email] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Telefono] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[Direccion] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
	[IdEmpresa] [int] NULL,
 CONSTRAINT [PK__Cliente__D5946642A508F043] PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Mtro].[Departamento]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Mtro].[Departamento](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[Descripcion] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[IdPais] [int] NULL,
	[CodPais] [varchar](3) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Departam__3214EC073572D19D] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Mtro].[Distrito]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Mtro].[Distrito](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[Descripcion] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[IdProvincia] [int] NULL,
	[CodProvincia] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[CodigoUbigeo] [varchar](6) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Distrito__3214EC07889F6F1E] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Mtro].[Empresa]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Mtro].[Empresa](
	[IdEmpresa] [int] IDENTITY(1,1) NOT NULL,
	[CodEmpresa] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[IdUbigeo] [int] NULL,
	[Ubigeo] [varchar](6) COLLATE Modern_Spanish_CI_AS NULL,
	[Ruc] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[RazonSocial] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[RazonComercial] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Telefono] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[Fax] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[Direccion] [varchar](120) COLLATE Modern_Spanish_CI_AS NULL,
	[DomicilioFiscal] [varchar](120) COLLATE Modern_Spanish_CI_AS NULL,
	[Urbanizacion] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[PaginaWeb] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[Email] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
	[Id_TDI] [int] NULL,
	[TpoLogin] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[Url_CompanyLogo] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[Url_CompanyConsult] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Empresa__5EF4033EF3D15E98] PRIMARY KEY CLUSTERED 
(
	[IdEmpresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Mtro].[Estado]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Mtro].[Estado](
	[IdEstado] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Estado__FBB0EDC1527CC90C] PRIMARY KEY CLUSTERED 
(
	[IdEstado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Mtro].[Pais]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Mtro].[Pais](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](6) COLLATE Modern_Spanish_CI_AS NULL,
	[Descripcion] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[Cod_Pais] [varchar](10) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Pais__3214EC071B7073F1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Mtro].[Provincia]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Mtro].[Provincia](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
	[Descripcion] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[IdDepartamento] [int] NULL,
	[CodDepartamento] [varchar](2) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Provinci__3214EC070E219910] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Mtro].[Ubigeo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Mtro].[Ubigeo](
	[IdUbigeo] [int] IDENTITY(1,1) NOT NULL,
	[CodigoUbigeo] [varchar](6) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[Descripcion] [varchar](100) COLLATE Modern_Spanish_CI_AS NOT NULL,
 CONSTRAINT [PK__Ubigeo__F682D161932BB05E] PRIMARY KEY CLUSTERED 
(
	[IdUbigeo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[Correo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[Correo](
	[IdEmail] [int] IDENTITY(1,1) NOT NULL,
	[IdEmpresa] [int] NULL,
	[Email] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[Password] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[DOMAIN] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[IP] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[PORT] [int] NULL,
	[PORT_ALTERN] [int] NULL,
	[RucEmpresa] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
	[TypeMail] [varchar](10) COLLATE Modern_Spanish_CI_AS NULL,
	[UseSSL] [int] NULL,
 CONSTRAINT [PK__Correo__E80F8BD4B1C29ED2] PRIMARY KEY CLUSTERED 
(
	[IdEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[Log_SlinAde]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[Log_SlinAde](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FECHA] [datetime] NULL,
	[MODULO] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[MESSAGE_ERROR] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[INNER_EXCEPTION] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[LogLogueo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[LogLogueo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FECHA] [datetime] NULL,
	[USERNAME] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[USERNAME_LOG] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[IDTIPOLOG] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[Menu]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[Menu](
	[IdMenu] [int] IDENTITY(1,1) NOT NULL,
	[NombreMenu] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[CodigoMenu] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[PadreMenu] [int] NULL,
	[NivelMenu] [int] NULL,
	[IdEstado] [int] NULL,
 CONSTRAINT [PK__Menus__4D7EA8E1CD4898DF] PRIMARY KEY CLUSTERED 
(
	[IdMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[MenuPerfil]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Seg].[MenuPerfil](
	[IdMenuPerfil] [int] IDENTITY(1,1) NOT NULL,
	[IdMenu] [int] NULL,
	[IdPerfil] [int] NULL,
 CONSTRAINT [PK__MenuPerf__CB60EC5073522BC8] PRIMARY KEY CLUSTERED 
(
	[IdMenuPerfil] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Seg].[Perfil]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[Perfil](
	[IdPerfil] [int] IDENTITY(1,1) NOT NULL,
	[NombrePerfil] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Codigo] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[RucEntity] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Perfil__C7BD5CC15D743D41] PRIMARY KEY CLUSTERED 
(
	[IdPerfil] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[RegistroLogueo]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[RegistroLogueo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FECHA] [datetime] NULL,
	[DNI_USUARIO] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[USERNAME] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[IDPERFIL] [int] NULL,
	[FECHAINGRESO] [datetime] NULL,
	[FECHASALIDA] [datetime] NULL,
	[HOSTNAME] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[SIP] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[IDTPOLOG] [int] NULL,
	[RucEntity] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEmpresa] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[Rol]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[Rol](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[NombreRol] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
	[CodigoRol] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Rol__2A49584C730F5EE0] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[Sede]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[Sede](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Cod] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[Name] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
	[RucEntity] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[SETUP]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[SETUP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[COD_MB] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[HOST] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[COD_HD] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[KY] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[ENCRYPT] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[COD_MC] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__SETUP__3214EC079035D4C4] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[TipoLog]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[TipoLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[COD] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[DESCRIPCION] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[TpoEntityxforLogin]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[TpoEntityxforLogin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TpoLogin] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[Descripcion] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[UsuarioRol]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[UsuarioRol](
	[IdUsuarioRol] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NULL,
	[IdRol] [int] NULL,
	[IdEstado] [int] NULL,
	[Dni] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__UsuarioR__6806BF4A534F4DB3] PRIMARY KEY CLUSTERED 
(
	[IdUsuarioRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[Usuarios]    Script Date: 10/04/2017 18:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[IdEstado] [int] NULL,
	[IdEmpresa] [int] NULL,
	[IdPerfil] [int] NULL,
	[Nombres] [varchar](60) COLLATE Modern_Spanish_CI_AS NULL,
	[ApePaterno] [varchar](60) COLLATE Modern_Spanish_CI_AS NULL,
	[ApeMaterno] [varchar](60) COLLATE Modern_Spanish_CI_AS NULL,
	[DNI_RUC] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[Direccion] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Telefono] [varchar](15) COLLATE Modern_Spanish_CI_AS NULL,
	[Email] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
	[Username] [varchar](30) COLLATE Modern_Spanish_CI_AS NULL,
	[Password] [varchar](max) COLLATE Modern_Spanish_CI_AS NULL,
	[FechaExpiracion] [datetime] NULL,
	[FechaRegistro] [datetime] NULL,
	[Sede] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Usuarios__5B65BF978C2FDAF8] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [Conf].[AmbienteTrabj] ON 

INSERT [Conf].[AmbienteTrabj] ([ID], [COD], [DESCRIPCION], [IDAMBIENTE], [RUCENTITY]) VALUES (1, N'DEV', N'Test', 2, N'RUCREPLACE')

SET IDENTITY_INSERT [Conf].[AmbienteTrabj] OFF

SET IDENTITY_INSERT [Conf].[DocumentoAmbiente] ON 

INSERT [Conf].[DocumentoAmbiente] ([ID], [ID_TIPO_CE], [TIPO_CE], [DESCR_TPO_CE], [IDAMBIENTE], [IDESTADO], [RUCENTITY]) VALUES (1, 1, N'01', N'Factura', 1, 1, N'RUCREPLACE')
INSERT [Conf].[DocumentoAmbiente] ([ID], [ID_TIPO_CE], [TIPO_CE], [DESCR_TPO_CE], [IDAMBIENTE], [IDESTADO], [RUCENTITY]) VALUES (2, 2, N'03', N'Boleta de venta', 1, 1, N'RUCREPLACE')
INSERT [Conf].[DocumentoAmbiente] ([ID], [ID_TIPO_CE], [TIPO_CE], [DESCR_TPO_CE], [IDAMBIENTE], [IDESTADO], [RUCENTITY]) VALUES (3, 3, N'07', N'Nota de credito', 1, 1, N'RUCREPLACE')
INSERT [Conf].[DocumentoAmbiente] ([ID], [ID_TIPO_CE], [TIPO_CE], [DESCR_TPO_CE], [IDAMBIENTE], [IDESTADO], [RUCENTITY]) VALUES (4, 4, N'08', N'Nota de debito', 1, 1, N'RUCREPLACE')
INSERT [Conf].[DocumentoAmbiente] ([ID], [ID_TIPO_CE], [TIPO_CE], [DESCR_TPO_CE], [IDAMBIENTE], [IDESTADO], [RUCENTITY]) VALUES (5, 5, N'09', N'Guia de remision remitente', 1, 3, N'RUCREPLACE')
INSERT [Conf].[DocumentoAmbiente] ([ID], [ID_TIPO_CE], [TIPO_CE], [DESCR_TPO_CE], [IDAMBIENTE], [IDESTADO], [RUCENTITY]) VALUES (6, 9, N'20', N'Documento de Retención', 1, 1, N'RUCREPLACE')
INSERT [Conf].[DocumentoAmbiente] ([ID], [ID_TIPO_CE], [TIPO_CE], [DESCR_TPO_CE], [IDAMBIENTE], [IDESTADO], [RUCENTITY]) VALUES (7, 10, N'40', N'Documento de Percepción', 1, 2, N'RUCREPLACE')

SET IDENTITY_INSERT [Conf].[DocumentoAmbiente] OFF


SET IDENTITY_INSERT [Conf].[TimeService] ON 

INSERT [Conf].[TimeService] ([Id], [CodeService], [NameService], [ValueTime], [IntervalValue], [MaxNumAttempts], [RucEntity], [IdEstado], [SubType]) VALUES (1, N'ADE.Services.SunatDelivery', N'Reenvío de Documentos', N'-', N'1', 1, N'RUCREPLACE', 1, N'WS')
INSERT [Conf].[TimeService] ([Id], [CodeService], [NameService], [ValueTime], [IntervalValue], [MaxNumAttempts], [RucEntity], [IdEstado], [SubType]) VALUES (2, N'ADE.Services.SummaryDelivery', N'Resumen de Boletas', N'19:22', N'-', 10, N'RUCREPLACE', 1, N'RC')
INSERT [Conf].[TimeService] ([Id], [CodeService], [NameService], [ValueTime], [IntervalValue], [MaxNumAttempts], [RucEntity], [IdEstado], [SubType]) VALUES (3, N'ADE.Services.SummaryDelivery', N'Resumen de Bajas', N'20:40', N'-', 10, N'RUCREPLACE', 3, N'RA')
INSERT [Conf].[TimeService] ([Id], [CodeService], [NameService], [ValueTime], [IntervalValue], [MaxNumAttempts], [RucEntity], [IdEstado], [SubType]) VALUES (4, N'ADE.Services.SummaryDelivery', N'Resumen de Reversiones', N'21:10', N'-', 10, N'RUCREPLACE', 3, N'RR')
INSERT [Conf].[TimeService] ([Id], [CodeService], [NameService], [ValueTime], [IntervalValue], [MaxNumAttempts], [RucEntity], [IdEstado], [SubType]) VALUES (5, N'ADE.Services.SummaryDelivery', N'Alerta de Documentos Pendientes', N'21:00', N'5', 50, N'RUCREPLACE', 2, N'SA')
INSERT [Conf].[TimeService] ([Id], [CodeService], [NameService], [ValueTime], [IntervalValue], [MaxNumAttempts], [RucEntity], [IdEstado], [SubType]) VALUES (6, N'ADE.Services.SendDocument', N'Envío de Documentos Electronicos', N'20:00', N'11', 0, N'RUCREPLACE', 1, N'WS')
INSERT [Conf].[TimeService] ([Id], [CodeService], [NameService], [ValueTime], [IntervalValue], [MaxNumAttempts], [RucEntity], [IdEstado], [SubType]) VALUES (7, N'ADE.Services.PrintDocument', N'Impresión de Documentos', N'', N'0', 0, N'RUCREPLACE', 1, N'WS')
INSERT [Conf].[TimeService] ([Id], [CodeService], [NameService], [ValueTime], [IntervalValue], [MaxNumAttempts], [RucEntity], [IdEstado], [SubType]) VALUES (8, N'ADE.Services.DownloadDocument', N'Descarga de Documentos', N'20', N'2', 0, N'RUCREPLACE', 1, N'WS')

SET IDENTITY_INSERT [Conf].[TimeService] OFF

SET IDENTITY_INSERT [Conf].[Tpo_Amb_Trabj] ON 

INSERT [Conf].[Tpo_Amb_Trabj] ([IDAMBIENTE], [COD], [DESCRIPCION], [IDESTADO]) VALUES (1, N'PRD', N'Producción', 1)
INSERT [Conf].[Tpo_Amb_Trabj] ([IDAMBIENTE], [COD], [DESCRIPCION], [IDESTADO]) VALUES (2, N'DEV', N'Test', 1)
INSERT [Conf].[Tpo_Amb_Trabj] ([IDAMBIENTE], [COD], [DESCRIPCION], [IDESTADO]) VALUES (3, N'HML', N'Homologación', 1)
SET IDENTITY_INSERT [Conf].[Tpo_Amb_Trabj] OFF
SET IDENTITY_INSERT [Conf].[URL_SUNAT] ON 

INSERT [Conf].[URL_SUNAT] ([ID_URL], [COD_URL], [DESCR_URL], [URL], [IDESTADO], [IDAMBIENTE], [NAME], [DOCUMENTOS]) VALUES (1, N'CE', N'Factura, Boleta de Venta, Nota de Credito, Nota de Debito', N'https://e-factura.sunat.gob.pe/ol-ti-itcpfegem/billService', 1, 1, NULL, N'01|03|07|08')
INSERT [Conf].[URL_SUNAT] ([ID_URL], [COD_URL], [DESCR_URL], [URL], [IDESTADO], [IDAMBIENTE], [NAME], [DOCUMENTOS]) VALUES (2, N'GUIA', N'Guia de Remisión', N'https://e-factura.sunat.gob.pe/ol-ti-itemision-guia-gem/billService', 1, 1, NULL, N'09')
INSERT [Conf].[URL_SUNAT] ([ID_URL], [COD_URL], [DESCR_URL], [URL], [IDESTADO], [IDAMBIENTE], [NAME], [DOCUMENTOS]) VALUES (3, N'CPE, CRE', N'Retención, Percepción', N'https://e-factura.sunat.gob.pe/ol-ti-itemision-otroscpe-gem/billService', 1, 1, NULL, N'20|40')
INSERT [Conf].[URL_SUNAT] ([ID_URL], [COD_URL], [DESCR_URL], [URL], [IDESTADO], [IDAMBIENTE], [NAME], [DOCUMENTOS]) VALUES (4, N'CE', N'Factura, Boleta de Venta, Nota de Credito, Nota de Debito', N'https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService', 1, 2, NULL, N'01|03|07|08')
INSERT [Conf].[URL_SUNAT] ([ID_URL], [COD_URL], [DESCR_URL], [URL], [IDESTADO], [IDAMBIENTE], [NAME], [DOCUMENTOS]) VALUES (5, N'GUIA', N'Guia de Remisión', N'https://e-beta.sunat.gob.pe/ol-ti-itemision-guia-gem-beta/billService', 1, 2, NULL, N'09')
INSERT [Conf].[URL_SUNAT] ([ID_URL], [COD_URL], [DESCR_URL], [URL], [IDESTADO], [IDAMBIENTE], [NAME], [DOCUMENTOS]) VALUES (6, N'CPE, CRE', N'Retención, Percepción', N'https://e-beta.sunat.gob.pe/ol-ti-itemision-otroscpe-gem-beta/billService', 1, 2, NULL, N'20|40')
INSERT [Conf].[URL_SUNAT] ([ID_URL], [COD_URL], [DESCR_URL], [URL], [IDESTADO], [IDAMBIENTE], [NAME], [DOCUMENTOS]) VALUES (7, N'CE,GUIA,CPE,CRE', N'Todos los Documentos', N'https://www.sunat.gob.pe:443/ol-ti-itcpgem-sqa/billService', 1, 3, NULL, N'01|03|07|08')
INSERT [Conf].[URL_SUNAT] ([ID_URL], [COD_URL], [DESCR_URL], [URL], [IDESTADO], [IDAMBIENTE], [NAME], [DOCUMENTOS]) VALUES (8, N'CE, GUIA, CPE, CRE', N'Consulta con Nro Atención', N'https://www.sunat.gob.pe:443/ol-it-wsconscpegem/billConsultService', 1, 1, NULL, N'Q')
SET IDENTITY_INSERT [Conf].[URL_SUNAT] OFF
SET IDENTITY_INSERT [Ctl].[CalculoISC] ON 

INSERT [Ctl].[CalculoISC] ([Id_ISC], [Code], [Desc], [Obsv], [IdEstado]) VALUES (1, N'01', N'Sistema al Valor', NULL, NULL)
INSERT [Ctl].[CalculoISC] ([Id_ISC], [Code], [Desc], [Obsv], [IdEstado]) VALUES (2, N'02', N'Aplicacion del monto Fijo', NULL, NULL)
INSERT [Ctl].[CalculoISC] ([Id_ISC], [Code], [Desc], [Obsv], [IdEstado]) VALUES (3, N'03', N'Sistema de Precios de Venta al Publico', NULL, NULL)
SET IDENTITY_INSERT [Ctl].[CalculoISC] OFF
SET IDENTITY_INSERT [Ctl].[ConceptosTributarios] ON 

INSERT [Ctl].[ConceptosTributarios] ([Id_CT], [Code], [Desc], [Obsv], [IdEstado]) VALUES (1, N'1001', N'Total valor de Venta - Operaciones gravadas', NULL, NULL)
INSERT [Ctl].[ConceptosTributarios] ([Id_CT], [Code], [Desc], [Obsv], [IdEstado]) VALUES (2, N'1002', N'Total valor de venta - Operaciones Inafectas', NULL, NULL)
INSERT [Ctl].[ConceptosTributarios] ([Id_CT], [Code], [Desc], [Obsv], [IdEstado]) VALUES (3, N'1003', N'Total valor de venta - Operaciones Exoneradas', NULL, NULL)
INSERT [Ctl].[ConceptosTributarios] ([Id_CT], [Code], [Desc], [Obsv], [IdEstado]) VALUES (4, N'1004', N'Operaciones Gratuitas', NULL, NULL)
INSERT [Ctl].[ConceptosTributarios] ([Id_CT], [Code], [Desc], [Obsv], [IdEstado]) VALUES (5, N'1005', N'Subtotal de venta', NULL, NULL)
INSERT [Ctl].[ConceptosTributarios] ([Id_CT], [Code], [Desc], [Obsv], [IdEstado]) VALUES (6, N'2001', N'Percepciones', NULL, NULL)
INSERT [Ctl].[ConceptosTributarios] ([Id_CT], [Code], [Desc], [Obsv], [IdEstado]) VALUES (7, N'2002', N'Retenciones', NULL, NULL)
INSERT [Ctl].[ConceptosTributarios] ([Id_CT], [Code], [Desc], [Obsv], [IdEstado]) VALUES (8, N'2003', N'Detracciones', NULL, NULL)
INSERT [Ctl].[ConceptosTributarios] ([Id_CT], [Code], [Desc], [Obsv], [IdEstado]) VALUES (9, N'2004', N'Bonificaciones', NULL, NULL)
INSERT [Ctl].[ConceptosTributarios] ([Id_CT], [Code], [Desc], [Obsv], [IdEstado]) VALUES (10, N'2005', N'Total Descuentos', NULL, NULL)
SET IDENTITY_INSERT [Ctl].[ConceptosTributarios] OFF
SET IDENTITY_INSERT [Ctl].[ElementoFacturaBoleta] ON 

INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (1, N'1000', N'Monto en letras', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (2, N'1002', N'Leyenda "Transferencia gratuita de un bien y/o servicio prestado"', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (3, N'2000', N'Leyenda "Comprobante de Percepcion"', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (4, N'2001', N'Leyenda "Bienes transferidos en la Amazonia region Selva para ser Consumidos en la misma"', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (5, N'2002', N'Leyenda "Servicios prestados en la Amazonia region Selva para ser Consumidos en la misma"', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (6, N'2003', N'Leyenda "Contratos de Construccion ejecutados en la Amazonia Region Selva"', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (7, N'2004', N'Leyenda "Agencia de Viaje - Paquete Turistico"', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (8, N'3000', N'Detracciones: codigo de BB y SS sujetos a Detraccion', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (9, N'3001', N'Detracciones: Numero de Cta en el BN', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (10, N'3002', N'Detracciones: Recursos Hidrobiologicos- Nombre y matricula de la embarcacion', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (11, N'3003', N'Detracciones: Recursos Hidrobiologicos- Tipo y cantidad de especie vendida', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (12, N'3004', N'Detracciones: Recursos Hidrobiologicos - Lugar de descarga', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (13, N'3005', N'Detracciones: Recursos Hidrobiologicos - Fecha de descarga', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (14, N'3006', N'Detracciones: Transporte Bienes via terrestre - Numero Registro MTC', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (15, N'3007', N'Detracciones: Transporte Bienes via terrestre - Configuracion Vehicular', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (16, N'3008', N'Detracciones: Transporte Bienes via terrestre - punto de origen', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (17, N'3009', N'Detracciones: Transporte Bienes via Terrestre - punto destino', NULL, NULL)
INSERT [Ctl].[ElementoFacturaBoleta] ([Id_EFB], [Code], [Descr], [Obsv], [IdEstado]) VALUES (18, N'3010', N'Detracciones: Transporte Bienes via terrestre - valor referencial preliminar', NULL, NULL)
SET IDENTITY_INSERT [Ctl].[ElementoFacturaBoleta] OFF
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0100', N'El sistema no puede responder su solicitud. Intente nuevamente o comuníquese con su Administrador ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0101', N'El encabezado de seguridad es incorrecto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0102', N'Usuario o contraseña incorrectos ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0103', N'El Usuario ingresado no existe ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0104', N'La Clave ingresada es incorrecta ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0105', N'El Usuario no está activo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0106', N'El Usuario no es válido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0109', N'El sistema no puede responder su solicitud. (El servicio de autenticación no está disponible) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0110', N'No se pudo obtener la informacion del tipo de usuario ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0111', N'No tiene el perfil para enviar comprobantes electronicos ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0112', N'El usuario debe ser secundario ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0113', N'El usuario no esta afiliado a Factura Electronica ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0125', N'No se pudo obtener la constancia ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0126', N'El ticket no le pertenece al usuario ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0127', N'El ticket no existe ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0130', N'El sistema no puede responder su solicitud. (No se pudo obtener el ticket de proceso) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0131', N'El sistema no puede responder su solicitud. (No se pudo grabar el archivo en el directorio) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0132', N'El sistema no puede responder su solicitud. (No se pudo grabar escribir en el archivo zip) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0133', N'El sistema no puede responder su solicitud. (No se pudo grabar la entrada del log) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0134', N'El sistema no puede responder su solicitud. (No se pudo grabar en el storage) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0135', N'El sistema no puede responder su solicitud. (No se pudo encolar el pedido) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0136', N'El sistema no puede responder su solicitud. (No se pudo recibir una respuesta del batch) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0137', N'El sistema no puede responder su solicitud. (Se obtuvo una respuesta nula) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0138', N'El sistema no puede responder su solicitud. (Error en Base de Datos)')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0151', N'El nombre del archivo ZIP es incorrecto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0152', N'No se puede enviar por este método un archivo de resumen ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0153', N'No se puede enviar por este método un archivo por lotes ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0154', N'El RUC del archivo no corresponde al RUC del usuario ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0155', N'El archivo ZIP esta vacio ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0156', N'El archivo ZIP esta corrupto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0157', N'El archivo ZIP no contiene comprobantes ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0158', N'El archivo ZIP contiene demasiados comprobantes para este tipo de envío ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0159', N'El nombre del archivo XML es incorrecto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0160', N'El archivo XML esta vacio ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0161', N'El nombre del archivo XML no coincide con el nombre del archivo ZIP ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0200', N'No se pudo procesar su solicitud. (Ocurrio un error en el batch) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0201', N'No se pudo procesar su solicitud. (Llego un requerimiento nulo al batch)')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0202', N'No se pudo procesar su solicitud. (No llego información del archivo ZIP)')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0203', N'No se pudo procesar su solicitud. (No se encontro archivos en la informacion del archivo ZIP)')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0204', N'No se pudo procesar su solicitud. (Este tipo de requerimiento solo acepta 1 archivo)')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0250', N'No se pudo procesar su solicitud. (Ocurrio un error desconocido al hacer unzip) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0251', N'No se pudo procesar su solicitud. (No se pudo crear un directorio para el unzip) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0252', N'No se pudo procesar su solicitud. (No se encontro archivos dentro del zip) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0401', N'El caso de prueba no existe ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0402', N'La numeracion o nombre del documento ya ha sido enviado anteriormente ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0403', N'El documento afectado por la nota no existe ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'0404', N'El documento afectado por la nota se encuentra rechazado ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1001', N'ID - El dato SERIE-CORRELATIVO no cumple con el formato de acuerdo al tipo de comprobante ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1002', N'El XML no contiene informacion en el tag ID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1003', N'InvoiceTypeCode - El valor del tipo de documento es invalido o no coincide con el nombre del archivo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1004', N'El XML no contiene el tag o no existe informacion de InvoiceTypeCode ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1005', N'CustomerAssignedAccountID - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1006', N'El XML no contiene el tag o no existe informacion de CustomerAssignedAccountID del emisor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1007', N'AdditionalAccountID - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1008', N'El XML no contiene el tag o no existe informacion de AdditionalAccountID del emisor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1009', N'IssueDate - El dato ingresado  no cumple con el patron YYYY-MM-DD')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1010', N'El XML no contiene el tag IssueDate')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1011', N'IssueDate- El dato ingresado no es valido')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1012', N'ID - El dato ingresado no cumple con el patron SERIE-CORRELATIVO')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1013', N'El XML no contiene informacion en el tag ID')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1014', N'CustomerAssignedAccountID - El dato ingresado no cumple con el estándar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1015', N'El XML no contiene el tag o no existe informacion de CustomerAssignedAccountID del emisor del documento')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1016', N'AdditionalAccountID - El dato ingresado no cumple con el estándar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1017', N'El XML no contiene el tag AdditionalAccountID del emisor del documento')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1018', N'IssueDate - El dato ingresado no cumple con el patron YYYY-MM-DD')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1019', N'El XML no contiene el tag IssueDate')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1020', N'IssueDate- El dato ingresado no es valido')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1021', N'Error en la validacion de la nota de credito')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1022', N'La serie o numero del documento modificado por la Nota Electrónica no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1023', N'No se ha especificado el tipo de documento modificado por la Nota electronica ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1024', N'CustomerAssignedAccountID - El dato ingresado no cumple con el estándar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1025', N'El XML no contiene el tag o no existe informacion de CustomerAssignedAccountID del emisor del documento')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1026', N'AdditionalAccountID - El dato ingresado no cumple con el estándar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1027', N'El XML no contiene el tag AdditionalAccountID del emisor del documento')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1028', N'IssueDate - El dato ingresado no cumple con el patron YYYY-MM-DD')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1029', N'El XML no contiene el tag IssueDate')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1030', N'IssueDate- El dato ingresado no es valido')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1031', N'Error en la validacion de la nota de debito')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1032', N'El comprobante fue informado previamente en una comunicacion de baja ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1033', N'El comprobante fue registrado previamente con otros datos ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1034', N'Número de RUC del nombre del archivo no coincide con el consignado en el contenido del archivo XML ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1035', N'Numero de Serie del nombre del archivo no coincide con el consignado en el contenido del archivo XML ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1036', N'Número de documento en el nombre del archivo no coincide con el consignado en el contenido del XML ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1037', N'El XML no contiene el tag o no existe informacion de RegistrationName del emisor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1038', N'RegistrationName - El nombre o razon social del emisor no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1039', N'Solo se pueden recibir notas electronicas que modifican facturas ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'1040', N'El tipo de documento modificado por la nota electronica no es valido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2010', N'El contribuyente no esta activo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2011', N'El contribuyente no esta habido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2012', N'El contribuyente no está autorizado a emitir comprobantes electrónicos')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2013', N'El contribuyente no cumple con tipo de empresa o tributos requeridos ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2014', N'El XML no contiene el tag o no existe informacion de CustomerAssignedAccountID del receptor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2015', N'El XML no contiene el tag o no existe informacion de AdditionalAccountID del receptor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2016', N'AdditionalAccountID - El dato ingresado en el tipo de documento de identidad del receptor no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2017', N'CustomerAssignedAccountID - El numero de documento de identidad del recepetor debe ser RUC ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2018', N'CustomerAssignedAccountID -  El dato ingresado no cumple con el estándar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2019', N'El XML no contiene el tag o no existe informacion de RegistrationName del emisor del documento')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2020', N'RegistrationName - El nombre o razon social del emisor no cumple con el estándar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2021', N'El XML no contiene el tag o no existe informacion de RegistrationName del receptor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2022', N'RegistrationName - El dato ingresado no cumple con el estandar ')
GO
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2023', N'El Numero de orden del item no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2024', N'El XML no contiene el tag InvoicedQuantity en el detalle de los Items ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2025', N'InvoicedQuantity El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2026', N'El XML no contiene el tag cac:Item/cbc:Description en el detalle de los Items')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2027', N'El XML no contiene el tag o no existe informacion de cac:Item/cbc:Description del item ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2028', N'Debe existir el tag cac:AlternativeConditionPrice con un elemento cbc:PriceTypeCode con valor 01 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2029', N'PriceTypeCode El dato ingresado no cumple con el estándar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2030', N'El XML no contiene el tag cbc:PriceTypeCode')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2031', N'LineExtensionAmount El dato ingresado no cumple con el estándar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2032', N'El XML no contiene el tag LineExtensionAmount en el detalle de los Items')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2033', N'El dato ingresado en TaxAmount de la linea no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2034', N'TaxAmount es obligatorio')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2035', N'cac:TaxCategory/cac:TaxScheme/cbc:ID El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2036', N'El codigo del tributo es invalido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2037', N'El XML no contiene el tag cac:TaxCategory/cac:TaxScheme/cbc:ID del Item')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2038', N'cac:TaxScheme/cbc:Name del item - No existe el tag o el dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2039', N'El XML no contiene el tag cac:TaxCategory/cac:TaxScheme/cbc:Name del Item')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2040', N'El tipo de afectacion del IGV es incorrecto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2041', N'El sistema de calculo del ISC es incorrecto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2042', N'Debe indicar el IGV. Es un campo obligatorio ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2043', N'El dato ingresado en PayableAmount no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2044', N'PayableAmount es obligatorio')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2045', N'El valor ingresado en AdditionalMonetaryTotal/cbc:ID es incorrecto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2046', N'AdditionalMonetaryTotal/cbc:ID debe tener valor')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2047', N'Es obligatorio al menos un AdditionalMonetaryTotal con codigo 1001, 1002 o 1003 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2048', N'El dato ingresado en TaxAmount no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2049', N'TaxAmount es obligatorio')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2050', N'TaxScheme ID - No existe el tag o el dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2051', N'El codigo del tributo es invalido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2052', N'El XML no contiene el tag TaxScheme ID de impuestos globales')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2053', N'TaxScheme Name - No existe el tag o el dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2054', N'El XML no contiene el tag TaxScheme Name de impuestos globales')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2055', N'TaxScheme TaxTypeCode - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2056', N'El XML no contiene el tag TaxScheme TaxTypeCode de impuestos globales')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2057', N'El Name o TaxTypeCode debe corresponder con el Id para el IGV ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2058', N'El Name o TaxTypeCode debe corresponder con el Id para el ISC ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2059', N'El dato ingresado en TaxSubtotal/cbc:TaxAmount no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2060', N'TaxSubtotal/cbc:TaxAmount es obligatorio')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2061', N'El tag global cac:TaxTotal/cbc:TaxAmount debe tener el mismo valor que cac:TaxTotal/cac:Subtotal/cbc:TaxAmount ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2062', N'El dato ingresado en PayableAmount no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2063', N'El XML no contiene el tag PayableAmount')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2064', N'El dato ingresado en ChargeTotalAmount no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2065', N'El dato ingresado en el campo Total Descuentos no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2066', N'Debe indicar una descripcion para el tag sac:AdditionalProperty/cbc:Value')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2067', N'cac:Price/cbc:PriceAmount - El dato ingresado no cumple con el estandar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2068', N'El XML no contiene el tag cac:Price/cbc:PriceAmount en el detalle de los Items ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2069', N'DocumentCurrencyCode - El dato ingresado no cumple con la estructura ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2070', N'El XML no contiene el tag o no existe informacion de DocumentCurrencyCode ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2071', N'La moneda debe ser la misma en todo el documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2072', N'CustomizationID - La versión del documento no es la correcta ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2073', N'El XML no contiene el tag o no existe informacion de CustomizationID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2074', N'UBLVersionID - La versión del UBL no es correcta ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2075', N'El XML no contiene el tag o no existe informacion de UBLVersionID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2076', N'cac:Signature/cbc:ID - Falta el identificador de la firma ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2077', N'El tag cac:Signature/cbc:ID debe contener informacion ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2078', N'cac:Signature/cac:SignatoryParty/cac:PartyIdentification/cbc:ID - Debe ser igual al RUC del emisor ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2079', N'El XML no contiene el tag cac:Signature/cac:SignatoryParty/cac:PartyIdentification/cbc:ID')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2080', N'cac:Signature/cac:SignatoryParty/cac:PartyName/cbc:Name - No cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2081', N'El XML no contiene el tag cac:Signature/cac:SignatoryParty/cac:PartyName/cbc:Name')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2082', N'cac:Signature/cac:DigitalSignatureAttachment/cac:ExternalReference/cbc:URI - No cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2083', N'El XML no contiene el tag cac:Signature/cac:DigitalSignatureAttachment/cac:ExternalReference/cbc:URI ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2084', N'ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/ds:Signature/@Id - No cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2085', N'El XML no contiene el tag ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/ds:Signature/@Id ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2086', N'ext:UBLExtensions/.../ds:Signature/ds:SignedInfo/ds:CanonicalizationMethod/@Algorithm - No cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2087', N'El XML no contiene el tag ext:UBLExtensions/.../ds:Signature/ds:SignedInfo/ds:CanonicalizationMethod/@Algorithm ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2088', N'ext:UBLExtensions/.../ds:Signature/ds:SignedInfo/ds:SignatureMethod/@Algorithm - No cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2089', N'El XML no contiene el tag ext:UBLExtensions/.../ds:Signature/ds:SignedInfo/ds:SignatureMethod/@Algorithm ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2090', N'ext:UBLExtensions/.../ds:Signature/ds:SignedInfo/ds:Reference/@URI - Debe estar vacio para id ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2091', N'El XML no contiene el tag ext:UBLExtensions/.../ds:Signature/ds:SignedInfo/ds:Reference/@URI')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2092', N'ext:UBLExtensions/.../ds:Signature/ds:SignedInfo/.../ds:Transform@Algorithm - No cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2093', N'El XML no contiene el tag ext:UBLExtensions/.../ds:Signature/ds:SignedInfo/ds:Reference/ds:Transform@Algorithm ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2094', N'ext:UBLExtensions/.../ds:Signature/ds:SignedInfo/ds:Reference/ds:DigestMethod/@Algorithm - No cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2095', N'El XML no contiene el tag ext:UBLExtensions/.../ds:Signature/ds:SignedInfo/ds:Reference/ds:DigestMethod/@Algorithm ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2096', N'ext:UBLExtensions/.../ds:Signature/ds:SignedInfo/ds:Reference/ds:DigestValue - No cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2097', N'El XML no contiene el tag ext:UBLExtensions/.../ds:Signature/ds:SignedInfo/ds:Reference/ds:DigestValue ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2098', N'ext:UBLExtensions/.../ds:Signature/ds:SignatureValue - No cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2099', N'El XML no contiene el tag ext:UBLExtensions/.../ds:Signature/ds:SignatureValue ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2100', N'ext:UBLExtensions/.../ds:Signature/ds:KeyInfo/ds:X509Data/ds:X509Certificate - No cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2101', N'El XML no contiene el tag ext:UBLExtensions/.../ds:Signature/ds:KeyInfo/ds:X509Data/ds:X509Certificate ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2102', N'Error al procesar la factura')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2103', N'La serie ingresada no es válida')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2104', N'Numero de RUC del emisor no existe')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2105', N'Factura a dar de baja no se encuentra registrada en SUNAT ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2106', N'Factura a dar de baja ya se encuentra en estado de baja ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2107', N'Numero de RUC SOL no coincide con RUC emisor')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2108', N'Presentacion fuera de fecha ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2109', N'El comprobante fue registrado previamente con otros datos')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2110', N'UBLVersionID - La versión del UBL no es correcta')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2111', N'El XML no contiene el tag o no existe informacion de UBLVersionID')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2112', N'CustomizationID - La version del documento no es correcta')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2113', N'El XML no contiene el tag o no existe informacion de CustomizationID')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2114', N'DocumentCurrencyCode -  El dato ingresado no cumple con la estructura')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2115', N'El XML no contiene el tag o no existe informacion de DocumentCurrencyCode')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2116', N'El tipo de documento modificado por la Nota de credito debe ser factura electronica o ticket ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2117', N'La serie o numero del documento modificado por la Nota de Credito no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2118', N'Debe indicar las facturas relacionadas a la Nota de Credito ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2119', N'La factura relacionada en la Nota de credito no esta registrada. ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2120', N'La factura relacionada en la nota de credito se encuentra de baja ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2121', N'La factura relacionada en la nota de credito esta registrada como rechazada ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2122', N'El tag cac:LegalMonetaryTotal/cbc:PayableAmount debe tener informacion valida')
GO
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2123', N'RegistrationName -  El dato ingresado no cumple con el estandar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2124', N'El XML no contiene el tag RegistrationName del emisor del documento')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2125', N'ReferenceID - El dato ingresado debe indicar SERIE-CORRELATIVO del documento al que se relaciona la Nota ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2126', N'El XML no contiene informacion en el tag ReferenceID del documento al que se relaciona la nota ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2127', N'ResponseCode - El dato ingresado no cumple con la estructura ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2128', N'El XML no contiene el tag o no existe informacion de ResponseCode ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2129', N'AdditionalAccountID - El dato ingresado en el tipo de documento de identidad del receptor no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2130', N'El XML no contiene el tag o no existe informacion de AdditionalAccountID del receptor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2131', N'CustomerAssignedAccountID - El numero de documento de identidad del receptor debe ser RUC ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2132', N'El XML no contiene el tag o no existe informacion de CustomerAssignedAccountID del receptor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2133', N'RegistrationName - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2134', N'El XML no contiene el tag o no existe informacion de RegistrationName del receptor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2135', N'cac:DiscrepancyResponse/cbc:Description - El dato ingresado no cumple con la estructura ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2136', N'El XML no contiene el tag o no existe informacion de cac:DiscrepancyResponse/cbc:Description ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2137', N'El Número de orden del item no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2138', N'CreditedQuantity/@unitCode - El dato ingresado no cumple con el estandar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2139', N'CreditedQuantity - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2140', N'El PriceTypeCode debe tener el valor 01 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2141', N'cac:TaxCategory/cac:TaxScheme/cbc:ID - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2142', N'El codigo del tributo es invalido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2143', N'cac:TaxScheme/cbc:Name del item - No existe el tag o el dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2144', N'cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode El dato ingresado no cumple con el estandar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2145', N'El tipo de afectacion del IGV es incorrecto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2146', N'El Nombre Internacional debe ser VAT')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2147', N'El sistema de calculo del ISC es incorrecto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2148', N'El Nombre Internacional debe ser EXC ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2149', N'El dato ingresado en PayableAmount no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2150', N'El valor ingresado en AdditionalMonetaryTotal/cbc:ID es incorrecto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2151', N'AdditionalMonetaryTotal/cbc:ID debe tener valor ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2152', N'Es obligatorio al menos un AdditionalInformation')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2153', N'Error al procesar la Nota de Credito')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2154', N'TaxAmount - El dato ingresado en impuestos globales no cumple con el estandar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2155', N'El XML no contiene el tag TaxAmount de impuestos globales')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2156', N'TaxScheme ID - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2157', N'El codigo del tributo es invalido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2158', N'El XML no contiene el tag o no existe informacion de TaxScheme ID de impuestos globales ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2159', N'TaxScheme Name - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2160', N'El XML no contiene el tag o no existe informacion de TaxScheme Name de impuestos globales ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2161', N'CustomizationID - La version del documento no es correcta')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2162', N'El XML no contiene el tag o no existe informacion de CustomizationID')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2163', N'UBLVersionID - La versión del UBL no es correcta')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2164', N'El XML no contiene el tag o no existe informacion de UBLVersionID')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2165', N'Error al procesar la Nota de Debito')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2166', N'RegistrationName - El dato ingresado no cumple con el estandar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2167', N'El XML no contiene el tag RegistrationName del emisor del documento')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2168', N'DocumentCurrencyCode -  El dato ingresado no cumple con el formato establecido')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2169', N'El XML no contiene el tag o no existe informacion de DocumentCurrencyCode')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2170', N'ReferenceID - El dato ingresado debe indicar SERIE-CORRELATIVO del documento al que se relaciona la Nota ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2171', N'El XML no contiene informacion en el tag ReferenceID del documento al que se relaciona la nota ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2172', N'ResponseCode - El dato ingresado no cumple con la estructura ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2173', N'El XML no contiene el tag o no existe informacion de ResponseCode ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2174', N'cac:DiscrepancyResponse/cbc:Description - El dato ingresado no cumple con la estructura ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2175', N'El XML no contiene el tag o no existe informacion de cac:DiscrepancyResponse/cbc:Description ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2176', N'AdditionalAccountID - El dato ingresado en el tipo de documento de identidad del receptor no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2177', N'El XML no contiene el tag o no existe informacion de AdditionalAccountID del receptor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2178', N'CustomerAssignedAccountID - El numero de documento de identidad del receptor debe ser RUC. ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2179', N'El XML no contiene el tag o no existe informacion de CustomerAssignedAccountID del receptor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2180', N'RegistrationName - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2181', N'El XML no contiene el tag o no existe informacion de RegistrationName del receptor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2182', N'TaxScheme ID - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2183', N'El codigo del tributo es invalido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2184', N'El XML no contiene el tag o no existe informacion de TaxScheme ID de impuestos globales ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2185', N'TaxScheme Name - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2186', N'El XML no contiene el tag o no existe informacion de TaxScheme Name de impuestos globales ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2187', N'El Numero de orden del item no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2188', N'DebitedQuantity/@unitCode El dato ingresado no cumple con el estandar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2189', N'DebitedQuantity El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2190', N'El XML no contiene el tag Price/cbc:PriceAmount en el detalle de los Items ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2191', N'El XML no contiene el tag Price/cbc:LineExtensionAmount en el detalle de los Items')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2192', N'EL PriceTypeCode debe tener el valor 01 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2193', N'cac:TaxCategory/cac:TaxScheme/cbc:ID El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2194', N'El codigo del tributo es invalido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2195', N'cac:TaxScheme/cbc:Name del item - No existe el tag o el dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2196', N'cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode El dato ingresado no cumple con el estandar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2197', N'El tipo de afectacion del IGV es incorrecto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2198', N'El Nombre Internacional debe ser VAT')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2199', N'El sistema de calculo del ISC es incorrecto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2200', N'El Nombre Internacional debe ser EXC')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2201', N'El tag cac:RequestedMonetaryTotal/cbc:PayableAmount debe tener informacion valida')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2202', N'TaxAmount - El dato ingresado en impuestos globales no cumple con el estándar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2203', N'El XML no contiene el tag TaxAmount de impuestos globales')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2204', N'El tipo de documento modificado por la Nota de Debito debe ser factura electronica o ticket ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2205', N'La serie o numero del documento modificado por la Nota de Debito no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2206', N'Debe indicar los documentos afectados por la Nota de Debito ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2207', N'La factura relacionada en la nota de debito se encuentra de baja ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2208', N'La factura relacionada en la nota de debito esta registrada como rechazada ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2209', N'La factura relacionada en la Nota de debito no esta registrada ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2210', N'El dato ingresado no cumple con el formato RC-fecha-correlativo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2211', N'El XML no contiene el tag ID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2212', N'UBLVersionID - La versión del UBL del resumen de boletas no es correcta')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2213', N'El XML no contiene el tag UBLVersionID')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2214', N'CustomizationID - La versión del resumen de boletas no es correcta')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2215', N'El XML no contiene el tag CustomizationID')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2216', N'CustomerAssignedAccountID - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2217', N'El XML no contiene el tag CustomerAssignedAccountID del emisor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2218', N'AdditionalAccountID - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2219', N'El XML no contiene el tag AdditionalAccountID del emisor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2220', N'El ID debe coincidir con el nombre del archivo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2221', N'El RUC debe coincidir con el RUC del nombre del archivo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2222', N'El contribuyente no está autorizado a emitir comprobantes electronicos')
GO
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2223', N'El archivo ya fue presentado anteriormente ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2224', N'Numero de RUC SOL no coincide con RUC emisor')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2225', N'Numero de RUC del emisor no existe')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2226', N'El contribuyente no esta activo')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2227', N'El contribuyente no cumple con tipo de empresa o tributos requeridos')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2228', N'RegistrationName - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2229', N'El XML no contiene el tag RegistrationName del emisor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2230', N'IssueDate - El dato ingresado no cumple con el patron YYYY-MM-DD')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2231', N'El XML no contiene el tag IssueDate')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2232', N'IssueDate- El dato ingresado no es valido')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2233', N'ReferenceDate - El dato ingresado no cumple con el patron YYYY-MM-DD ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2234', N'El XML no contiene el tag ReferenceDate')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2235', N'ReferenceDate- El dato ingresado no es valido')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2236', N'La fecha del IssueDate no debe ser mayor al Today ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2237', N'La fecha del ReferenceDate no debe ser mayor al Today ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2238', N'LineID - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2239', N'LineID - El dato ingresado debe ser correlativo mayor a cero ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2240', N'El XML no contiene el tag LineID de SummaryDocumentsLine ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2241', N'DocumentTypeCode - El valor del tipo de documento es invalido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2242', N'El XML no contiene el tag DocumentTypeCode ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2243', N'El dato ingresado no cumple con el patron SERIE ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2244', N'El XML no contiene el tag DocumentSerialID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2245', N'El dato ingresado en StartDocumentNumberID debe ser numerico ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2246', N'El XML no contiene el tag StartDocumentNumberID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2247', N'El dato ingresado en sac:EndDocumentNumberID debe ser numerico ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2248', N'El XML no contiene el tag sac:EndDocumentNumberID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2249', N'Los rangos deben ser mayores a cero ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2250', N'En el rango de comprobantes, el EndDocumentNumberID debe ser mayor o igual al StartInvoiceNumberID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2251', N'El dato ingresado en TotalAmount debe ser numerico mayor o igual a cero ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2252', N'El XML no contiene el tag TotalAmount')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2253', N'El dato ingresado en TotalAmount debe ser numerico mayor a cero')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2254', N'PaidAmount - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2255', N'El XML no contiene el tag PaidAmount ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2256', N'InstructionID - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2257', N'El XML no contiene el tag InstructionID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2258', N'Debe indicar Referencia de Importes asociados a las boletas de venta')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2259', N'Debe indicar 3 Referencias de Importes asociados a las boletas de venta ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2260', N'PaidAmount - El dato ingresado debe ser mayor o igual a 0.00')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2261', N'cbc:Amount - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2262', N'El XML no contiene el tag cbc:Amount')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2263', N'ChargeIndicator - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2264', N'El XML no contiene el tag ChargeIndicator')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2265', N'Debe indicar Información acerca del Importe Total de Otros Cargos ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2266', N'Debe indicar cargos mayores o iguales a cero')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2267', N'TaxScheme ID - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2268', N'El codigo del tributo es invalido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2269', N'El XML no contiene el tag TaxScheme ID de Información acerca del importe total de un tipo particular de impuesto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2270', N'TaxScheme Name - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2271', N'El XML no contiene el tag TaxScheme Name de impuesto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2272', N'TaxScheme TaxTypeCode - El dato ingresado no cumple con el estandar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2273', N'TaxAmount - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2274', N'El XML no contiene el tag TaxAmount')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2275', N'Si el codigo de tributo es 2000, el nombre del tributo debe ser ISC ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2276', N'Si el codigo de tributo es 1000, el nombre del tributo debe ser IGV ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2277', N'No se ha consignado ninguna informacion del importe total de tributos ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2278', N'Debe indicar Información acerca del importe total de ISC e IGV ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2279', N'Debe indicar Items de consolidado de documentos')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2280', N'Existen problemas con la informacion del resumen de comprobantes')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2281', N'Error en la validacion de los rangos de los comprobantes')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2282', N'Existe documento ya informado anteriormente ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2283', N'El dato ingresado no cumple con el formato RA-fecha-correlativo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2284', N'El XML no contiene el tag ID')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2285', N'El ID debe coincidir con el nombre del archivo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2286', N'El RUC debe coincidir con el RUC del nombre del archivo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2287', N'AdditionalAccountID - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2288', N'El XML no contiene el tag AdditionalAccountID del emisor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2289', N'CustomerAssignedAccountID - El dato ingresado no cumple con el estándar')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2290', N'El XML no contiene el tag CustomerAssignedAccountID del emisor del documento')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2291', N'El contribuyente no esta autorizado a emitir comprobantes electrónicos')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2292', N'Numero de RUC SOL no coincide con RUC emisor')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2293', N'Numero de RUC del emisor no existe')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2294', N'El contribuyente no esta activo')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2295', N'El contribuyente no cumple con tipo de empresa o tributos requeridos')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2296', N'RegistrationName - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2297', N'El XML no contiene el tag RegistrationName del emisor del documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2298', N'IssueDate - El dato ingresado no cumple con el patron YYYY-MM-DD')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2299', N'El XML no contiene el tag IssueDate ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2300', N'IssueDate - El dato ingresado no es valido')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2301', N'La fecha del IssueDate no debe ser mayor al Today ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2302', N'ReferenceDate - El dato ingresado no cumple con el patron YYYY-MM-DD ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2303', N'El XML no contiene el tag ReferenceDate')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2304', N'ReferenceDate - El dato ingresado no es valido')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2305', N'LineID - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2306', N'LineID - El dato ingresado debe ser correlativo mayor a cero ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2307', N'El XML no contiene el tag LineID de VoidedDocumentsLine ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2308', N'DocumentTypeCode - El valor del tipo de documento es invalido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2309', N'El XML no contiene el tag DocumentTypeCode ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2310', N'El dato ingresado no cumple con el patron SERIE ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2311', N'El XML no contiene el tag DocumentSerialID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2312', N'El dato ingresado en DocumentNumberID debe ser numerico y como maximo de 8 digitos ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2313', N'El XML no contiene el tag DocumentNumberID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2314', N'El dato ingresado en VoidReasonDescription debe contener información válida ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2315', N'El XML no contiene el tag VoidReasonDescription ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2316', N'Debe indicar Items en VoidedDocumentsLine')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2317', N'Error al procesar el resumen de anulados')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2318', N'CustomizationID - La version del documento no es correcta')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2319', N'El XML no contiene el tag CustomizationID')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2320', N'UBLVersionID - La version del UBL  no es la correcta')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2321', N'El XML no contiene el tag UBLVersionID')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2322', N'Error en la validacion de los rangos')
GO
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2323', N'Existe documento ya informado anteriormente en una comunicacion de baja ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2324', N'El archivo de comunicacion de baja ya fue presentado anteriormente ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2325', N'El certificado usado no es el comunicado a SUNAT ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2326', N'El certificado usado se encuentra de baja ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2327', N'El certificado usado no se encuentra vigente ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2328', N'El certificado usado se encuentra revocado ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2329', N'La fecha de emision se encuentra fuera del limite permitido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2330', N'La fecha de generación de la comunicación debe ser igual a la fecha consignada en el nombre del archivo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2331', N'Número de RUC del nombre del archivo no coincide con el consignado en el contenido del archivo XML')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2332', N'Número de Serie del nombre del archivo no coincide con el consignado en el contenido del archivo XML')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2333', N'Número de documento en el nombre del archivo no coincide con el consignado en el contenido del XML')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2334', N'El documento electrónico ingresado ha sido alterado ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2335', N'El documento electrónico ingresado ha sido alterado ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2336', N'Ocurrió un error en el proceso de validación de la firma digital ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2337', N'La moneda debe ser la misma en todo el documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2338', N'La moneda debe ser la misma en todo el documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2339', N'El dato ingresado en PayableAmount no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2340', N'El valor ingresado en AdditionalMonetaryTotal/cbc:ID es incorrecto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2341', N'AdditionalMonetaryTotal/cbc:ID debe tener valor ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2342', N'Fecha de emision de la factura no coincide con la informada en la comunicacion ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2343', N'cac:TaxTotal/cac:TaxSubtotal/cbc:TaxAmount - El dato ingresado no cumple con el estandar ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2344', N'El XML no contiene el tag cac:TaxTotal/cac:TaxSubtotal/cbc:TaxAmount')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2345', N'La serie no corresponde al tipo de comprobante ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2346', N'La fecha de generación del resumen debe ser igual a la fecha consignada en el nombre del archivo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2347', N'Los rangos informados en el archivo XML se encuentran duplicados o superpuestos')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2348', N'Los documentos informados en el archivo XML se encuentran duplicados')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2349', N'Debe consignar solo un elemento sac:AdditionalMonetaryTotal con cbc:ID igual a 1001 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2350', N'Debe consignar solo un elemento sac:AdditionalMonetaryTotal con cbc:ID igual a 1002 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2351', N'Debe consignar solo un elemento sac:AdditionalMonetaryTotal con cbc:ID igual a 1003 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2352', N'Debe consignar solo un elemento cac:TaxTotal a nivel global para IGV (cbc:ID igual a 1000) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2353', N'Debe consignar solo un elemento cac:TaxTotal a nivel global para ISC (cbc:ID igual a 2000) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2354', N'Debe consignar solo un elemento cac:TaxTotal a nivel global para Otros (cbc:ID igual a 9999) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2355', N'Debe consignar solo un elemento cac:TaxTotal a nivel de item para IGV (cbc:ID igual a 1000) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2356', N'Debe consignar solo un elemento cac:TaxTotal a nivel de item para ISC (cbc:ID igual a 2000) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2357', N'Debe consignar solo un elemento sac:BillingPayment a nivel de item con cbc:InstructionID igual a 01 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2358', N'Debe consignar solo un elemento sac:BillingPayment a nivel de item con cbc:InstructionID igual a 02 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2359', N'Debe consignar solo un elemento sac:BillingPayment a nivel de item con cbc:InstructionID igual a 03 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2360', N'Debe consignar solo un elemento sac:BillingPayment a nivel de item con cbc:InstructionID igual a 04')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2361', N'Debe consignar solo un elemento cac:TaxTotal a nivel de item para Otros (cbc:ID igual a 9999) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2362', N'Debe consignar solo un tag cac:AccountingSupplierParty/cbc:AdditionalAccountID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2363', N'Debe consignar solo un tag cac:AccountingCustomerParty/cbc:AdditionalAccountID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2364', N'El comprobante contiene un tipo y número de Guía de Remisión repetido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2365', N'El comprobante contiene un tipo y número de Documento Relacionado repetido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2366', N'El codigo en el tag sac:AdditionalProperty/cbc:ID debe tener 4 posiciones')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2367', N'El dato ingresado en PriceAmount del Precio de venta unitario por item no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2368', N'El dato ingresado en TaxSubtotal/cbc:TaxAmount del item no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2369', N'El dato ingresado en PriceAmount del Valor de venta unitario por item no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2370', N'El dato ingresado en LineExtensionAmount del item no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2371', N'El XML no contiene el tag cbc:TaxExemptionReasonCode de Afectacion al IGV ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2372', N'El tag en el item cac:TaxTotal/cbc:TaxAmount debe tener el mismo valor que cac:TaxTotal/cac:TaxSubtotal/cbc:TaxAmount ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2373', N'Si existe monto de ISC en el ITEM debe especificar el sistema de calculo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2374', N'La factura a dar de baja tiene una fecha de recepcion fuera del plazo permitido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2375', N'Fecha de emision de la boleta no coincide con la fecha de emision consignada en la comunicacion ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2376', N'La boleta de venta a dar de baja fue informada en un resumen con fecha de recepcion fuera del plazo permitido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2377', N'El Name o TaxTypeCode debe corresponder con el Id para el IGV ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2378', N'El Name o TaxTypeCode debe corresponder con el Id para el ISC ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2379', N'La numeracion de boleta de venta a dar de baja fue generada en una fecha fuera del plazo permitido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2380', N'El documento tiene observaciones')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2381', N'Comprobante no cumple con el Grupo 1: No todos los items corresponden a operaciones gravadas a IGV ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2382', N'Comprobante no cumple con el Grupo 2: No todos los items corresponden a operaciones inafectas o exoneradas al IGV ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2383', N'Comprobante no cumple con el Grupo 3: Falta leyenda con codigo 1002 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2384', N'Comprobante no cumple con el Grupo 3: Existe item con operación onerosa ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2385', N'Comprobante no cumple con el Grupo 4: Debe exitir Total descuentos mayor a cero ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2386', N'Comprobante no cumple con el Grupo 5: Todos los items deben tener operaciones afectas a ISC ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2387', N'Comprobante no cumple con el Grupo 6: El monto de percepcion no existe o es cero ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2388', N'Comprobante no cumple con el Grupo 6: Todos los items deben tener código de Afectación al IGV igual a 10 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2389', N'Comprobante no cumple con el Grupo 7: El codigo de moneda no es diferente a PEN ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2390', N'Comprobante no cumple con el Grupo 8: No todos los items corresponden a operaciones gravadas a IGV ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2391', N'Comprobante no cumple con el Grupo 9: No todos los items corresponden a operaciones inafectas o exoneradas al IGV ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2392', N'Comprobante no cumple con el Grupo 10: Falta leyenda con codigo 1002 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2393', N'Comprobante no cumple con el Grupo 10: Existe item con operación onerosa ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2394', N'Comprobante no cumple con el Grupo 11: Debe existir Total descuentos mayor a cero ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2395', N'Comprobante no cumple con el Grupo 12: El codigo de moneda no es diferente a PEN ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2396', N'Si el monto total es mayor a S/. 700.00 debe consignar tipo y numero de documento del adquiriente ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2397', N'El tipo de documento del adquiriente no puede ser Numero de RUC ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2398', N'El documento a dar de baja se encuentra rechazado ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2399', N'El tipo de documento modificado por la Nota de credito debe ser boleta electronica')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2400', N'El tipo de documento modificado por la Nota de debito debe ser boleta electronica ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2401', N'No se puede leer (parsear) el archivo XML ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2402', N'El caso de prueba no existe')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2403', N'La numeracion o nombre del documento ya ha sido enviado anteriormente')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2404', N'Documento afectado por la nota electronica no se encuentra autorizado ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2405', N'Contribuyente no se encuentra autorizado como emisor de boletas electronicas ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2406', N'Existe mas de un tag sac:AdditionalMonetaryTotal con el mismo ID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2407', N'Existe mas de un tag sac:AdditionalProperty con el mismo ID ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2408', N'El dato ingresado en PriceAmount del Valor referencial unitario por item no cumple con el formato establecido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2409', N'Existe mas de un tag cac:AlternativeConditionPrice con el mismo cbc:PriceTypeCode ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2410', N'Se ha consignado un valor invalido en el campo cbc:PriceTypeCode ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2411', N'Ha consignado mas de un elemento cac:AllowanceCharge con el mismo campo cbc:ChargeIndicator ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2412', N'Se ha consignado mas de un documento afectado por la nota (tag cac:BillingReference) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2413', N'Se ha consignado mas de un motivo o sustento de la nota (tag cac:DiscrepancyResponse/cbc:Description) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2414', N'No se ha consignado en la nota el tag cac:DiscrepancyResponse ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2415', N'Se ha consignado en la nota mas de un tag cac:DiscrepancyResponse ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2416', N'Si existe leyenda Transferida Gratuita debe consignar Total Valor de Venta de Operaciones Gratuitas')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2417', N'Debe consignar Valor Referencial unitario por ítem en operaciones no onerosas')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2418', N'Si consigna Valor Referencial unitario por ítem en operaciones no onerosas, la operación debe ser no onerosa')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2419', N'El dato ingresado en AllowanceTotalAmount no cumple con el formato establecido')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'2420', N'Ya transcurrieron mas de 25 dias calendarios para concluir con su proceso de homologacion')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'253', N'No se pudo procesar su solicitud. (No se pudo comprimir la constancia) ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'300', N'No se encontró la raíz documento xml')
GO
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'301', N'Elemento raiz del xml no esta definido')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'302', N'Codigo del tipo de comprobante no registrado')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'303', N'No existe el directorio de schemas ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'304', N'No existe el archivo de schema ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'305', N'El sistema no puede procesar el archivo xml ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'306', N'No se puede leer (parsear) el archivo XML ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'307', N'No se pudo recuperar la constancia ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'400', N'No tiene permiso para enviar casos de pruebas ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4000', N'El documento ya fue presentado anteriormente.')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4001', N'El numero de RUC del receptor no existe. ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4002', N'Para el TaxTypeCode, esta usando un valor que no existe en el catalogo. ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4003', N'El comprobante fue registrado previamente como rechazado.')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4004', N'El DocumentTypeCode de las guias debe existir y tener 2 posiciones ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4005', N'El DocumentTypeCode de las guias debe ser 09 o 31 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4006', N'El ID de las guias debe tener informacion de la SERIE-NUMERO de guia. ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4007', N'El XML no contiene el ID de las guias.')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4008', N'El DocumentTypeCode de Otros documentos relacionados no cumple con el estandar. ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4009', N'El DocumentTypeCode de Otros documentos relacionados tiene valores incorrectos. ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4010', N'El ID de los documentos relacionados no cumplen con el estandar. ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4011', N'El XML no contiene el tag ID de documentos relacionados.')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4012', N'El ubigeo indicado en el comprobante no es el mismo que esta registrado para el contribuyente. ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4013', N'El RUC del receptor no esta activo ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4014', N'El RUC del receptor no esta habido ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4015', N'Si el tipo de documento del receptor no es RUC, debe tener operaciones de exportacion ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4016', N'El total valor venta neta de oper. gravadas IGV debe ser mayor a 0.00 o debe existir oper. gravadas onerosas ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4017', N'El total valor venta neta de oper. inafectas IGV debe ser mayor a 0.00 o debe existir oper. inafectas onerosas o de export. ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4018', N'El total valor venta neta de oper. exoneradas IGV debe ser mayor a 0.00 o debe existir oper. exoneradas ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4019', N'El calculo del IGV no es correcto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4020', N'El ISC no esta informado correctamente ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4021', N'Si se utiliza la leyenda con codigo 2000, el importe de percepcion debe ser mayor a 0.00 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4022', N'Si se utiliza la leyenda con código 2001, el total de operaciones exoneradas debe ser mayor a 0.00 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4023', N'Si se utiliza la leyenda con código 2002, el total de operaciones exoneradas debe ser mayor a 0.00 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4024', N'Si se utiliza la leyenda con código 2003, el total de operaciones exoneradas debe ser mayor a 0.00 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4025', N'Si usa la leyenda de Transferencia o Servivicio gratuito, todos los items deben ser no onerosos ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4026', N'No se puede indicar Guia de remision de remitente y Guia de remision de transportista en el mismo documento ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4027', N'El importe total no coincide con la sumatoria de los valores de venta mas los tributos mas los cargos ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4028', N'El monto total de la nota de credito debe ser menor o igual al monto de la factura ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4029', N'El ubigeo indicado en el comprobante no es el mismo que esta registrado para el contribuyente ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4030', N'El ubigeo indicado en el comprobante no es el mismo que esta registrado para el contribuyente ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4031', N'Debe indicar el nombre comercial ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4032', N'Si el código del motivo de emisión de la Nota de Credito es 03, debe existir la descripción del item ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4033', N'La fecha de generación de la numeración debe ser menor o igual a la fecha de generación de la comunicación ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4034', N'El comprobante fue registrado previamente como baja')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4035', N'El comprobante fue registrado previamente como rechazado')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4036', N'La fecha de emisión de los rangos debe ser menor o igual a la fecha de generación del resumen ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4037', N'El calculo del Total de IGV del Item no es correcto ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4038', N'El resumen contiene menos series por tipo de documento que el envío anterior para la misma fecha de emisión ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4039', N'No ha consignado información del ubigeo del domicilio fiscal ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4040', N'Si el importe de percepcion es mayor a 0.00, debe utilizar una leyenda con codigo 2000 ')
INSERT [Ctl].[ErroresRespuestaSunat] ([ERR_COD], [ERR_DES]) VALUES (N'4041', N'El codigo de pais debe ser PE ')
SET IDENTITY_INSERT [Ctl].[TipoAfectacionIGV] ON 

INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (1, N'10', N'Gravado Operacion Onerosa', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (2, N'11', N'Gravado-Retiro por premio', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (3, N'12', N'Gravado-Retiro por donacion ', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (4, N'13', N'Gravado - Retiro', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (5, N'14', N'Gravado - Retiro por publicidad', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (6, N'15', N'Gravado - Bonificaciones', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (7, N'16', N'Gravado - Retiro por entrega a trabajador', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (8, N'20', N'Exonerado - Operacion Onerosa', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (9, N'30', N'Inafecto - Operacion Onerosa', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (10, N'31', N'Inafecto - Retiro por Bonificacion', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (11, N'32', N'Inafecto - Retiro', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (12, N'33', N'Inafecto - Retiro por muestras medicas', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (13, N'34', N'Inafecto- Retiro por Convenio Colectivo', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (14, N'35', N'Inafecto - Retiro por premio', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (15, N'36', N'Inafecto - Retiro por Publicidad', NULL, NULL)
INSERT [Ctl].[TipoAfectacionIGV] ([Id_TAIGV], [Code], [Desc], [Obsv], [IdEstado]) VALUES (16, N'40', N'Exportacion', NULL, NULL)
SET IDENTITY_INSERT [Ctl].[TipoAfectacionIGV] OFF
SET IDENTITY_INSERT [Ctl].[TipoDocumento] ON 

INSERT [Ctl].[TipoDocumento] ([Id_TD], [Code], [Desc], [Un1001], [IdEstado], [Padre]) VALUES (1, N'01', N'Factura', N'380', 1, 0)
INSERT [Ctl].[TipoDocumento] ([Id_TD], [Code], [Desc], [Un1001], [IdEstado], [Padre]) VALUES (2, N'03', N'Boleta de venta', N'346', 1, 0)
INSERT [Ctl].[TipoDocumento] ([Id_TD], [Code], [Desc], [Un1001], [IdEstado], [Padre]) VALUES (3, N'07', N'Nota de credito', N'381', 1, 0)
INSERT [Ctl].[TipoDocumento] ([Id_TD], [Code], [Desc], [Un1001], [IdEstado], [Padre]) VALUES (4, N'08', N'Nota de debito', N'383', 1, 0)
INSERT [Ctl].[TipoDocumento] ([Id_TD], [Code], [Desc], [Un1001], [IdEstado], [Padre]) VALUES (5, N'09', N'Guia de remision remitente', NULL, 1, 0)
INSERT [Ctl].[TipoDocumento] ([Id_TD], [Code], [Desc], [Un1001], [IdEstado], [Padre]) VALUES (6, N'12', N'Ticket de maquina registradora', NULL, 1, 0)
INSERT [Ctl].[TipoDocumento] ([Id_TD], [Code], [Desc], [Un1001], [IdEstado], [Padre]) VALUES (7, N'18', N'Documentos emitidos por la AFP', NULL, 1, 0)
INSERT [Ctl].[TipoDocumento] ([Id_TD], [Code], [Desc], [Un1001], [IdEstado], [Padre]) VALUES (8, N'31', N'Guia de remision transportista', NULL, 1, 0)
INSERT [Ctl].[TipoDocumento] ([Id_TD], [Code], [Desc], [Un1001], [IdEstado], [Padre]) VALUES (9, N'20', N'Doc. Retención', NULL, 1, 0)
INSERT [Ctl].[TipoDocumento] ([Id_TD], [Code], [Desc], [Un1001], [IdEstado], [Padre]) VALUES (10, N'40', N'Doc. Percepción', NULL, 1, 0)
SET IDENTITY_INSERT [Ctl].[TipoDocumento] OFF
SET IDENTITY_INSERT [Ctl].[TipoDocumentoIdentidad] ON 

INSERT [Ctl].[TipoDocumentoIdentidad] ([Id_TDI], [Code], [Desc], [Obsv], [IdEstado], [Descripcion]) VALUES (1, N'0', N'Documento tributario no dom. sin ruc', NULL, 1, N'Documento tributario no dom. sin ruc')
INSERT [Ctl].[TipoDocumentoIdentidad] ([Id_TDI], [Code], [Desc], [Obsv], [IdEstado], [Descripcion]) VALUES (2, N'1', N'DNI', NULL, 1, N'Documento nacional de identidad')
INSERT [Ctl].[TipoDocumentoIdentidad] ([Id_TDI], [Code], [Desc], [Obsv], [IdEstado], [Descripcion]) VALUES (3, N'4', N'Carnet de extranjeria', NULL, 1, N'Carnet de extranjeria')
INSERT [Ctl].[TipoDocumentoIdentidad] ([Id_TDI], [Code], [Desc], [Obsv], [IdEstado], [Descripcion]) VALUES (4, N'6', N'RUC', NULL, 1, N'Registro unico de Contribuyentes')
INSERT [Ctl].[TipoDocumentoIdentidad] ([Id_TDI], [Code], [Desc], [Obsv], [IdEstado], [Descripcion]) VALUES (5, N'7', N'Pasaporte', NULL, 1, N'Pasaporte')
INSERT [Ctl].[TipoDocumentoIdentidad] ([Id_TDI], [Code], [Desc], [Obsv], [IdEstado], [Descripcion]) VALUES (6, N'A', N'Ced. diplomatica de identidad', NULL, 1, N'Ced. diplomatica de identidad')
SET IDENTITY_INSERT [Ctl].[TipoDocumentoIdentidad] OFF
SET IDENTITY_INSERT [Ctl].[TipoNotaCredito] ON 

INSERT [Ctl].[TipoNotaCredito] ([Id_NC], [Code], [Desc], [Obsv], [IdEstado]) VALUES (1, N'01', N'Anulacion de la Operacion', NULL, NULL)
INSERT [Ctl].[TipoNotaCredito] ([Id_NC], [Code], [Desc], [Obsv], [IdEstado]) VALUES (2, N'02', N'Anulacion por Error en el RUC', NULL, NULL)
INSERT [Ctl].[TipoNotaCredito] ([Id_NC], [Code], [Desc], [Obsv], [IdEstado]) VALUES (3, N'03', N'Correccion por error en la descripcion', NULL, NULL)
INSERT [Ctl].[TipoNotaCredito] ([Id_NC], [Code], [Desc], [Obsv], [IdEstado]) VALUES (4, N'04', N'Descuento global', NULL, NULL)
INSERT [Ctl].[TipoNotaCredito] ([Id_NC], [Code], [Desc], [Obsv], [IdEstado]) VALUES (5, N'05', N'Descuento por Item', NULL, NULL)
INSERT [Ctl].[TipoNotaCredito] ([Id_NC], [Code], [Desc], [Obsv], [IdEstado]) VALUES (6, N'06', N'Devolucion Total', NULL, NULL)
INSERT [Ctl].[TipoNotaCredito] ([Id_NC], [Code], [Desc], [Obsv], [IdEstado]) VALUES (7, N'07', N'Devolucion por Item', NULL, NULL)
INSERT [Ctl].[TipoNotaCredito] ([Id_NC], [Code], [Desc], [Obsv], [IdEstado]) VALUES (8, N'08', N'Bonificacion', NULL, NULL)
INSERT [Ctl].[TipoNotaCredito] ([Id_NC], [Code], [Desc], [Obsv], [IdEstado]) VALUES (9, N'09', N'Disminucion en el valor', NULL, NULL)
SET IDENTITY_INSERT [Ctl].[TipoNotaCredito] OFF
SET IDENTITY_INSERT [Ctl].[TipoTributos] ON 

INSERT [Ctl].[TipoTributos] ([Id_TT], [Code], [Desc], [UnEce5153], [Obsv], [IdEstado]) VALUES (1, N'1000', N'IGV Impuesto General de las Ventas', N'VAT', NULL, NULL)
INSERT [Ctl].[TipoTributos] ([Id_TT], [Code], [Desc], [UnEce5153], [Obsv], [IdEstado]) VALUES (2, N'2000', N'ISC Impuesto Selectivo al Consumo', N'EXC', NULL, NULL)
INSERT [Ctl].[TipoTributos] ([Id_TT], [Code], [Desc], [UnEce5153], [Obsv], [IdEstado]) VALUES (3, N'9999', N'Otros Conceptos de Pago', N'OTH', NULL, NULL)
SET IDENTITY_INSERT [Ctl].[TipoTributos] OFF
SET IDENTITY_INSERT [Fact].[Coin] ON 

INSERT [Fact].[Coin] ([Id], [Simbolo], [Descripcion], [CodeInternal]) VALUES (1, N'$', N'DOLAR', NULL)
INSERT [Fact].[Coin] ([Id], [Simbolo], [Descripcion], [CodeInternal]) VALUES (2, N'€', N'EURO', NULL)

SET IDENTITY_INSERT [Fact].[Coin] OFF




SET IDENTITY_INSERT [Fact].[O.EstadoDoc] ON 

INSERT [Fact].[O.EstadoDoc] ([Id_ED_DOC], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (1, N'Documento Anulado', N'Doc. Anul.', NULL, NULL, NULL)
SET IDENTITY_INSERT [Fact].[O.EstadoDoc] OFF
SET IDENTITY_INSERT [Fact].[O.EstadoDocumento] ON 

INSERT [Fact].[O.EstadoDocumento] ([Id_ED], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (1, N'Documento en BD', N'IBD', N'verde agua', N'El sistema ingreso la informacion de la interface en la base de datos', N'~/Img/estado/verdeagua.png')
INSERT [Fact].[O.EstadoDocumento] ([Id_ED], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (2, N'XML Generado', N'XGN', N'negro', N'El sistema genero el XML pero aun no se envia a SUNAT', N'~/Img/estado/negro.png')
INSERT [Fact].[O.EstadoDocumento] ([Id_ED], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (3, N'Enviado a Sunat', N'XES', N'crema', N'El  XML se envio a Sunat, aun no se obtiene respuesta', N'~/Img/estado/crema.png')
INSERT [Fact].[O.EstadoDocumento] ([Id_ED], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (4, N'Error de Envio', N'EEN', N'dorado', N'El XML no se pudo enviar por algun problema', N'~/Img/estado/dorado.png')
INSERT [Fact].[O.EstadoDocumento] ([Id_ED], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (5, N'Aceptado con Obs', N'SOB', N'celeste', N'El XML se envio a Sunat y la respuesta fue satisfactoria con algunas observaciones', N'~/Img/estado/celeste.png')
INSERT [Fact].[O.EstadoDocumento] ([Id_ED], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (6, N'Rechazado', N'SRE', N'rojo', N'El XML se envio a Sunat pero esta lo rechazo', N'~/Img/estado/rojo.png')
INSERT [Fact].[O.EstadoDocumento] ([Id_ED], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (7, N'Aceptado Correcto', N'SOK', N'verde', N'El XML se envio a Sunat y la respuesta fue satisfactoria sin errores', N'~/Img/estado/verde.png')
INSERT [Fact].[O.EstadoDocumento] ([Id_ED], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (8, N'Ticket Recibido', N'ESX', N'purpura', N'El Ticket está pendiente de consulta en a Sunat', N'~/Img/estado/purpura.png')
INSERT [Fact].[O.EstadoDocumento] ([Id_ED], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (9, N'Pendiente de Envío', N'PEN', N'naranja', N'EL XML está pendiente de envío a Sunat', N'~/Img/estado/naranja.png')
INSERT [Fact].[O.EstadoDocumento] ([Id_ED], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (10, N'Anulado pendiente de Envío', N'ANS', N'azul', N'El Documento a sido anulado en el SLIN-ADE', N'~/Img/estado/azul.png')
INSERT [Fact].[O.EstadoDocumento] ([Id_ED], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (14, N'Anulado enviado a Sunat', N'AES', N'rojo', N'El Documento anulado se a enviado a Sunat', N'~/Img/estado/rojo.png')
INSERT [Fact].[O.EstadoDocumento] ([Id_ED], [Desc], [Abrev], [Color], [DescGeneral], [RutaImagen]) VALUES (18, N'Anulado aceptado por Sunat', N'AAS', N'rojo', N'El Documento anulado a sido aceptado por Sunat', N'~/Img/estado/rojo.png')
SET IDENTITY_INSERT [Fact].[O.EstadoDocumento] OFF


SET IDENTITY_INSERT [Fact].[TipoDocumentPrint] ON 

INSERT [Fact].[TipoDocumentPrint] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (1, N'01', 7, N'RUCREPLACE')
INSERT [Fact].[TipoDocumentPrint] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (2, N'07', 7, N'RUCREPLACE')
INSERT [Fact].[TipoDocumentPrint] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (3, N'08', 7, N'RUCREPLACE')
INSERT [Fact].[TipoDocumentPrint] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (4, N'09', 7, N'RUCREPLACE')
INSERT [Fact].[TipoDocumentPrint] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (5, N'20', 7, N'RUCREPLACE')
INSERT [Fact].[TipoDocumentPrint] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (6, N'40', 7, N'RUCREPLACE')
INSERT [Fact].[TipoDocumentPrint] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (7, N'03', 2, N'RUCREPLACE')
SET IDENTITY_INSERT [Fact].[TipoDocumentPrint] OFF


SET IDENTITY_INSERT [Fact].[TipoDocumentSend] ON 

INSERT [Fact].[TipoDocumentSend] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (1, N'01', 7, N'RUCREPLACE')
INSERT [Fact].[TipoDocumentSend] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (2, N'07', 7, N'RUCREPLACE')
INSERT [Fact].[TipoDocumentSend] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (3, N'08', 7, N'RUCREPLACE')
INSERT [Fact].[TipoDocumentSend] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (4, N'09', 7, N'RUCREPLACE')
INSERT [Fact].[TipoDocumentSend] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (5, N'20', 7, N'RUCREPLACE')
INSERT [Fact].[TipoDocumentSend] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (6, N'40', 7, N'RUCREPLACE')
INSERT [Fact].[TipoDocumentSend] ([ID], [TIPO_CE], [IDESTADO], [RucEntity]) VALUES (7, N'03', 2, N'RUCREPLACE')

SET IDENTITY_INSERT [Fact].[TipoDocumentSend] OFF



SET IDENTITY_INSERT [Mtro].[Estado] ON 

INSERT [Mtro].[Estado] ([IdEstado], [Descripcion]) VALUES (1, N'Activo')
INSERT [Mtro].[Estado] ([IdEstado], [Descripcion]) VALUES (2, N'Inactivo')
INSERT [Mtro].[Estado] ([IdEstado], [Descripcion]) VALUES (3, N'Bloqueado')
SET IDENTITY_INSERT [Mtro].[Estado] OFF

SET IDENTITY_INSERT [Mtro].[Pais] ON 

INSERT [Mtro].[Pais] ([Id], [Codigo], [Descripcion], [Cod_Pais]) VALUES (1, N'051', N'Peru', N'PE')
SET IDENTITY_INSERT [Mtro].[Pais] OFF




SET IDENTITY_INSERT [Seg].[Menu] ON 

INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (1, N'Menu. Configuracion', N'MenuConfiguracion', NULL, 0, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (2, N'Menu. Envio', N'MenuEnvio', NULL, 0, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (3, N'Menu. Consultas', N'MenuConsultas', NULL, 0, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (4, N'Menu. Registro', N'MenuRegistro', NULL, 0, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (5, N'Menu. Seguridad', N'MenuSeguridad', NULL, 0, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (6, N'Menu. Mantenimiento', N'MenuMantenimiento', NULL, 0, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (7, N'Send. Envio Archivos', N'EnvioDocumento', 2, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (8, N'Send. Documentos Enviados', N'DocumentoEnviado', 2, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (9, N'Cons. Documentos Electrónicos CE', N'ConsultaDocumento', 3, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (10, N'Cons. Documentos Anulados', N'ConsultaDocumentoAnulado', 3, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (11, N'Cons. Resumen de Boletas', N'ConsultaResumenRC', 3, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (12, N'Reg.. Anular Documento', N'AnularDocumento', 4, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (13, N'Seg.. Crear Perfiles', N'CrearPerfil', 5, 1, 3)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (14, N'Seg.. Asignar Menu Perfil', N'AsignarPerfiles', 5, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (15, N'Mant. Registro Empresa', N'RegistroEmpresa', 6, 1, 3)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (16, N'Mant. Registro Usuario', N'RegistroUsuario', 5, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (17, N'Mant. Empresas', N'ListadoEmpresa', 6, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (18, N'Mant. Usuarios', N'ListadoUsuario', 6, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (19, N'Mant. Registro Correo', N'RegistroCorreo', 6, 1, 3)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (20, N'Mant. Correos', N'ListadoCorreo', 6, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (21, N'Cons. Resumen de Bajas', N'ConsultaResumenRA', 3, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (22, N'Mant. Registro Cliente', N'RegistroCliente', 6, 1, 3)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (23, N'Mant. Clientes', N'ListadoCliente', 6, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (24, N'Cons. Documentos Anulados ADE', N'ConsultaDocAnuladoADE', 3, 1, 3)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (25, N'Conf. URL SUNAT', N'ConfiguracionURL', 1, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (26, N'Conf. Impresión de Documentos', N'ConfiguracionImpresion', 1, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (27, N'Conf. Envío de Documentos', N'ConfiguracionEnvio', 1, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (28, N'Cons. Documento Electrónicos CRE', N'ConsultaDocumentoCRE', 3, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (29, N'Cons. Resumen de Reversiones', N'ConsultaResumenRR', 3, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (30, N'Conf. Ambiente de Trabajo', N'ConfiguracionAmbienteTrabajo', 1, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (31, N'Seg.. Lista Log Seguridad', N'ListLogSeguridad', 5, 1, 3)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (32, N'Conf. Hora de Servicio', N'ConfiguracionTimeService', 1, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (33, N'Conf. Tipo de Cambio', N'ConfiguracionTipoCambio', 1, 1, 3)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (34, N'Cons. Documentos Recibidos', N'ConsultaDocumentoReceived', 3, 1, 1)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (35, N'Menu. Ajuste Empresa', N'MenuAjusteEmpresa', NULL, 0, 3)
INSERT [Seg].[Menu] ([IdMenu], [NombreMenu], [CodigoMenu], [PadreMenu], [NivelMenu], [IdEstado]) VALUES (36, N'Comp. Cambiar Empresa', N'CambiarEmpresa', 35, 1, 3)
SET IDENTITY_INSERT [Seg].[Menu] OFF



SET IDENTITY_INSERT [Seg].[Perfil] ON 

INSERT [Seg].[Perfil] ([IdPerfil], [NombrePerfil], [Codigo], [RucEntity]) VALUES (1, N'Administrador', N'A', N'RUCREPLACE')
INSERT [Seg].[Perfil] ([IdPerfil], [NombrePerfil], [Codigo], [RucEntity]) VALUES (2, N'Facturador', N'F', N'RUCREPLACE')
INSERT [Seg].[Perfil] ([IdPerfil], [NombrePerfil], [Codigo], [RucEntity]) VALUES (3, N'Consultor', N'CC', N'RUCREPLACE')
INSERT [Seg].[Perfil] ([IdPerfil], [NombrePerfil], [Codigo], [RucEntity]) VALUES (4, N'Contabilidad', N'C', N'RUCREPLACE')

SET IDENTITY_INSERT [Seg].[Perfil] OFF



SET IDENTITY_INSERT [Seg].[Rol] ON 

INSERT [Seg].[Rol] ([IdRol], [NombreRol], [IdEstado], [CodigoRol]) VALUES (1, N'Guardar', 1, N'G')
INSERT [Seg].[Rol] ([IdRol], [NombreRol], [IdEstado], [CodigoRol]) VALUES (2, N'Modificar', 1, N'M')
INSERT [Seg].[Rol] ([IdRol], [NombreRol], [IdEstado], [CodigoRol]) VALUES (3, N'Buscar', 1, N'B')
INSERT [Seg].[Rol] ([IdRol], [NombreRol], [IdEstado], [CodigoRol]) VALUES (4, N'Exportar', 1, N'EX')
INSERT [Seg].[Rol] ([IdRol], [NombreRol], [IdEstado], [CodigoRol]) VALUES (5, N'Enviar', 1, N'EN')
INSERT [Seg].[Rol] ([IdRol], [NombreRol], [IdEstado], [CodigoRol]) VALUES (6, N'Nuevo', 1, N'N')
SET IDENTITY_INSERT [Seg].[Rol] OFF



SET IDENTITY_INSERT [Seg].[TipoLog] ON 

INSERT [Seg].[TipoLog] ([ID], [COD], [DESCRIPCION]) VALUES (1, N'OK', N'Exitoso')
INSERT [Seg].[TipoLog] ([ID], [COD], [DESCRIPCION]) VALUES (2, N'FAUL', N'Fallido')
SET IDENTITY_INSERT [Seg].[TipoLog] OFF



SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__Empresa__CAF036731D89A980]    Script Date: 10/04/2017 18:56:50 ******/
ALTER TABLE [Mtro].[Empresa] ADD  CONSTRAINT [UQ__Empresa__CAF036731D89A980] UNIQUE NONCLUSTERED 
(
	[Ruc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [Ctl].[CalculoISC]  WITH CHECK ADD  CONSTRAINT [FK__CalculoIS__IdEst__1273C1CD] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Ctl].[CalculoISC] CHECK CONSTRAINT [FK__CalculoIS__IdEst__1273C1CD]
GO
ALTER TABLE [Ctl].[ConceptosTributarios]  WITH CHECK ADD  CONSTRAINT [FK__Conceptos__IdEst__15502E78] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Ctl].[ConceptosTributarios] CHECK CONSTRAINT [FK__Conceptos__IdEst__15502E78]
GO
ALTER TABLE [Ctl].[DocumentoTributario]  WITH CHECK ADD  CONSTRAINT [FK__Documento__IdEst__182C9B23] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Ctl].[DocumentoTributario] CHECK CONSTRAINT [FK__Documento__IdEst__182C9B23]
GO
ALTER TABLE [Ctl].[ElementoFacturaBoleta]  WITH CHECK ADD  CONSTRAINT [FK__ElementoF__IdEst__1B0907CE] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Ctl].[ElementoFacturaBoleta] CHECK CONSTRAINT [FK__ElementoF__IdEst__1B0907CE]
GO
ALTER TABLE [Ctl].[TipoAfectacionIGV]  WITH CHECK ADD  CONSTRAINT [FK__TipoAfect__IdEst__1DE57479] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Ctl].[TipoAfectacionIGV] CHECK CONSTRAINT [FK__TipoAfect__IdEst__1DE57479]
GO
ALTER TABLE [Ctl].[TipoDocumento]  WITH CHECK ADD  CONSTRAINT [FK__TipoDocum__IdEst__20C1E124] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Ctl].[TipoDocumento] CHECK CONSTRAINT [FK__TipoDocum__IdEst__20C1E124]
GO
ALTER TABLE [Ctl].[TipoDocumentoIdentidad]  WITH CHECK ADD  CONSTRAINT [FK__TipoDocum__IdEst__239E4DCF] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Ctl].[TipoDocumentoIdentidad] CHECK CONSTRAINT [FK__TipoDocum__IdEst__239E4DCF]
GO
ALTER TABLE [Ctl].[TipoNotaCredito]  WITH CHECK ADD  CONSTRAINT [FK__TipoNotaC__IdEst__267ABA7A] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Ctl].[TipoNotaCredito] CHECK CONSTRAINT [FK__TipoNotaC__IdEst__267ABA7A]
GO
ALTER TABLE [Ctl].[TipoNotaDebito]  WITH CHECK ADD  CONSTRAINT [FK__TipoNotaD__IdEst__29572725] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Ctl].[TipoNotaDebito] CHECK CONSTRAINT [FK__TipoNotaD__IdEst__29572725]
GO
ALTER TABLE [Ctl].[TipoPrecioVentaUnitario]  WITH CHECK ADD  CONSTRAINT [FK__TipoPreci__IdEst__2C3393D0] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Ctl].[TipoPrecioVentaUnitario] CHECK CONSTRAINT [FK__TipoPreci__IdEst__2C3393D0]
GO
ALTER TABLE [Ctl].[TipoTributos]  WITH CHECK ADD  CONSTRAINT [FK__TipoTribu__IdEst__2F10007B] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Ctl].[TipoTributos] CHECK CONSTRAINT [FK__TipoTribu__IdEst__2F10007B]
GO
ALTER TABLE [Ctl].[TipoValorVenta]  WITH CHECK ADD  CONSTRAINT [FK__TipoValor__IdEst__31EC6D26] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Ctl].[TipoValorVenta] CHECK CONSTRAINT [FK__TipoValor__IdEst__31EC6D26]
GO
ALTER TABLE [Fact].[I.detalleDocumento]  WITH CHECK ADD FOREIGN KEY([Id_Bajadocu])
REFERENCES [Fact].[I.Bajadocumento] ([Id_Bajadocu])
GO
ALTER TABLE [Fact].[I.detalleDocumento]  WITH CHECK ADD FOREIGN KEY([Id_Bajadocu])
REFERENCES [Fact].[I.Bajadocumento] ([Id_Bajadocu])
GO
ALTER TABLE [Fact].[I.detalleDocumento]  WITH CHECK ADD FOREIGN KEY([Id_Bajadocu])
REFERENCES [Fact].[I.Bajadocumento] ([Id_Bajadocu])
GO
ALTER TABLE [Fact].[I.DocumentoCabecera]  WITH CHECK ADD FOREIGN KEY([Id_ED])
REFERENCES [Fact].[O.EstadoDocumento] ([Id_ED])
GO
ALTER TABLE [Fact].[I.DocumentoCabecera]  WITH CHECK ADD FOREIGN KEY([Id_ED])
REFERENCES [Fact].[O.EstadoDocumento] ([Id_ED])
GO
ALTER TABLE [Fact].[I.DocumentoCabecera]  WITH CHECK ADD FOREIGN KEY([Id_ED])
REFERENCES [Fact].[O.EstadoDocumento] ([Id_ED])
GO
ALTER TABLE [Fact].[O.DocumentoCabecera]  WITH CHECK ADD  CONSTRAINT [FK__O.Documen__Id_ED__0B5CAFEA] FOREIGN KEY([Id_ED])
REFERENCES [Fact].[O.EstadoDocumento] ([Id_ED])
GO
ALTER TABLE [Fact].[O.DocumentoCabecera] CHECK CONSTRAINT [FK__O.Documen__Id_ED__0B5CAFEA]
GO
ALTER TABLE [Fact].[O.DocumentoDetalle]  WITH CHECK ADD  CONSTRAINT [FK__O.Documen__Id_DC__0E391C95] FOREIGN KEY([Id_DC])
REFERENCES [Fact].[O.DocumentoCabecera] ([Id_DC])
GO
ALTER TABLE [Fact].[O.DocumentoDetalle] CHECK CONSTRAINT [FK__O.Documen__Id_DC__0E391C95]
GO
ALTER TABLE [Fact].[O.NotasRespuesta]  WITH CHECK ADD  CONSTRAINT [FK_O.NotasRespuesta_ErroresRespuestaSunat_ERR_COD] FOREIGN KEY([ERR_COD])
REFERENCES [Ctl].[ErroresRespuestaSunat] ([ERR_COD])
GO
ALTER TABLE [Fact].[O.NotasRespuesta] CHECK CONSTRAINT [FK_O.NotasRespuesta_ErroresRespuestaSunat_ERR_COD]
GO
ALTER TABLE [Fact].[O.Serie]  WITH CHECK ADD  CONSTRAINT [FK__O.Serie__Id_TD__47DBAE45] FOREIGN KEY([Id_TD])
REFERENCES [Ctl].[TipoDocumento] ([Id_TD])
GO
ALTER TABLE [Fact].[O.Serie] CHECK CONSTRAINT [FK__O.Serie__Id_TD__47DBAE45]
GO
ALTER TABLE [Fact].[O.Serie]  WITH CHECK ADD  CONSTRAINT [FK__O.Serie__IdEmpre__49C3F6B7] FOREIGN KEY([IdEmpresa])
REFERENCES [Mtro].[Empresa] ([IdEmpresa])
GO
ALTER TABLE [Fact].[O.Serie] CHECK CONSTRAINT [FK__O.Serie__IdEmpre__49C3F6B7]
GO
ALTER TABLE [Fact].[O.Serie]  WITH CHECK ADD  CONSTRAINT [FK__O.Serie__IdEstad__48CFD27E] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Fact].[O.Serie] CHECK CONSTRAINT [FK__O.Serie__IdEstad__48CFD27E]
GO
ALTER TABLE [Mtro].[Empresa]  WITH CHECK ADD  CONSTRAINT [FK__Empresa__IdEstad__3F466844] FOREIGN KEY([IdEstado])
REFERENCES [Mtro].[Estado] ([IdEstado])
GO
ALTER TABLE [Mtro].[Empresa] CHECK CONSTRAINT [FK__Empresa__IdEstad__3F466844]
GO
ALTER TABLE [Seg].[UsuarioRol]  WITH CHECK ADD  CONSTRAINT [FK__UsuarioRo__IdRol__00DF2177] FOREIGN KEY([IdRol])
REFERENCES [Seg].[Rol] ([IdRol])
GO
ALTER TABLE [Seg].[UsuarioRol] CHECK CONSTRAINT [FK__UsuarioRo__IdRol__00DF2177]
GO
ALTER TABLE [Seg].[UsuarioRol]  WITH CHECK ADD  CONSTRAINT [FK__UsuarioRo__IdRol__03BB8E22] FOREIGN KEY([IdRol])
REFERENCES [Seg].[Rol] ([IdRol])
GO
ALTER TABLE [Seg].[UsuarioRol] CHECK CONSTRAINT [FK__UsuarioRo__IdRol__03BB8E22]
GO
ALTER TABLE [Seg].[UsuarioRol]  WITH CHECK ADD  CONSTRAINT [FK__UsuarioRo__IdRol__0697FACD] FOREIGN KEY([IdRol])
REFERENCES [Seg].[Rol] ([IdRol])
GO
ALTER TABLE [Seg].[UsuarioRol] CHECK CONSTRAINT [FK__UsuarioRo__IdRol__0697FACD]
GO
ALTER TABLE [Seg].[UsuarioRol]  WITH CHECK ADD  CONSTRAINT [FK__UsuarioRo__IdRol__09746778] FOREIGN KEY([IdRol])
REFERENCES [Seg].[Rol] ([IdRol])
GO
ALTER TABLE [Seg].[UsuarioRol] CHECK CONSTRAINT [FK__UsuarioRo__IdRol__09746778]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Notas del XML de Respuesta de Sunat (Si es que hubiesen)' , @level0type=N'SCHEMA',@level0name=N'Fact', @level1type=N'TABLE',@level1name=N'O.NotasRespuesta'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla para guardar los Resumenes de Bajas.' , @level0type=N'SCHEMA',@level0name=N'Fact', @level1type=N'TABLE',@level1name=N'O.RBajasCabecera'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla para guardar los Resumenes de Boletas.' , @level0type=N'SCHEMA',@level0name=N'Fact', @level1type=N'TABLE',@level1name=N'O.RBajasDetalle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla para guardar los Resumenes de Boletas.' , @level0type=N'SCHEMA',@level0name=N'Fact', @level1type=N'TABLE',@level1name=N'O.RBoletasCabecera'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla para guardar los Resumenes de Boletas.' , @level0type=N'SCHEMA',@level0name=N'Fact', @level1type=N'TABLE',@level1name=N'O.RBoletasDetalle'
GO

