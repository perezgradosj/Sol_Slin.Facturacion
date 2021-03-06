/****** Object:  Schema [Mtro]    Script Date: 11/04/2017 14:13:21 ******/
CREATE SCHEMA [Mtro]
GO
/****** Object:  Schema [Seg]    Script Date: 11/04/2017 14:13:21 ******/
CREATE SCHEMA [Seg]
GO
/****** Object:  StoredProcedure [Mtro].[Usp_ActualizarClienteEmpresa]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Mtro].[Usp_ActualizarClienteEmpresa]
@IdUsuario [int],
@NroDocumento [varchar](11),
@RazonSocial [varchar](250),
@Email [varchar](250),
@Contrasenia [varchar](250),
--@Usuario [varchar](250),
@Password [varchar](250),
@IdEstado [int]
AS
BEGIN
	UPDATE [Mtro].[ClienteEmpresa] 
	SET	--[NroDocumento] = @NroDocumento
		[RazonSocial] = @RazonSocial
		,[Email] = @Email
		,[Contrasenia] = @Contrasenia --[Usuario] = @Usuario
		,[Password] = @Password
		,[IdEstado] = @IdEstado
	WHERE [IdUsuario] = @IdUsuario AND
		  [NroDocumento] = @NroDocumento
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_GetListaTipoUsuario]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Mtro].[Usp_GetListaTipoUsuario]
AS
BEGIN
	SELECT
		*
	FROM [Mtro].[TipoUsuario]
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_RegistrarClienteEmpresa]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Mtro].[Usp_RegistrarClienteEmpresa]
@NroDocumento [varchar](11),
@RazonSocial [varchar](250),
@Email [varchar](250),
--@Contrasenia [varchar](250),
@Username [varchar](250),
@Password [varchar](250),
@IdTipoUsuario [int],
@IdEstado [int],
@RucEmpresa varchar(11)
AS
BEGIN
	INSERT INTO 
		[Mtro].[ClienteEmpresa]
		([NroDocumento]
		,[RazonSocial]
		,[Email]
		--,[Contrasenia]
		,[Username]
		,[Password]
		,[IdTipoUsuario]
		,[IdEstado])
	VALUES 
		(@NroDocumento
		,@RazonSocial
		,@Email
		--,@Contrasenia
		,@Username
		,@Password
		,@IdTipoUsuario
		,@IdEstado)
END

GO
/****** Object:  StoredProcedure [Mtro].[Usp_ValidarNroDocumento]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Mtro].[Usp_ValidarNroDocumento]
@NroDocumento VARCHAR(11)
AS
BEGIN
	SELECT 
		*
	FROM [Mtro].[ClienteEmpresa]
	WHERE [NroDocumento] = @NroDocumento
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ActualizarContrasenia]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [Seg].[Usp_ActualizarContrasenia]
@IdUsuario INT,
--@Username VARCHAR(100),
@Password VARCHAR(250),
@NuevoPassword VARCHAR(250)
AS
BEGIN
	UPDATE [Mtro].[ClienteEmpresa]
		SET [Password] = @NuevoPassword
	WHERE IdUsuario = @IdUsuario
		  --[Password] = @Password
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_BloquearUsuario]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [Seg].[Usp_BloquearUsuario] --'',''
@Username VARCHAR(250)
AS
BEGIN
	UPDATE [Mtro].[ClienteEmpresa] SET IdEstado = 3
	WHERE [Username] = @Username
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_GetEntitySetup]    Script Date: 11/04/2017 14:13:21 ******/
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
		[RucEmpresa] AS RucEntity,
		[RazonSocial] AS RazonSocial
	FROM [Mtro].[Empresa]
	WHERE [RucEmpresa] = @EntityId
END
GO
/****** Object:  StoredProcedure [Seg].[Usp_GetInformationEntity]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_GetInformationEntity]
@EntityId VARCHAR(11)
AS
BEGIN
	SELECT
		[IdEmpresa] AS EntityId
		,[RucEmpresa] AS RucEntity
		,[RazonSocial]
		,[Direccion]
		,[Telefono]
		,[Email]
	FROM [Mtro].[Empresa]
	WHERE [RucEmpresa] = @EntityId
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_IniciarSesion]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [Seg].[Usp_IniciarSesion] --'12345678990','psjHj4O/TB8='
@Username VARCHAR(250),
@Password VARCHAR(250)
AS
BEGIN
	SELECT *
	FROM [Mtro].[ClienteEmpresa] AS MCE
	WHERE MCE.[Username] = @Username AND
		  MCE.[Password] = @Password AND
		  MCE.IdEstado = 1
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_ObtenerCredencialesdeEnvio]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_ObtenerCredencialesdeEnvio]
@EntityRuc VARCHAR(11)
AS
BEGIN
	SELECT 
		* 
	FROM [Mtro].[Empresa]
	WHERE [RucEmpresa] = @EntityRuc
END
GO
/****** Object:  StoredProcedure [Seg].[Usp_ObtenerUsuarioLogeado]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROC [Seg].[Usp_ObtenerUsuarioLogeado] --'admin','psjHj4O/TB8='
@Username VARCHAR(250),
@Password VARCHAR(250)
AS
BEGIN
	SELECT 
		*
	FROM [Mtro].[ClienteEmpresa] AS MCE --INNER JOIN
		 --[Mtro].[Estado] AS MES ON SU.IdEstado = MES.IdEstado INNER JOIN
		 --[Mtro].[Empresa] AS MEP ON SU.IdEmpresa = MEP.IdEmpresa INNER JOIN
		 --[Seg].[Correo] AS SC ON SU.IdEmpresa = SC.IdEmpresa
	WHERE MCE.[Username] = @Username AND
		  MCE.[Password] = @Password
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_RegisterComp]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_RegisterComp]
@RucComp VARCHAR(11),
@RazonSoc VARCHAR(250),
@Dir VARCHAR(250),
@Tel VARCHAR(20),
@Email VARCHAR(250),
@Pass VARCHAR(250),
@Dom VARCHAR(250),
@IP VARCHAR(40),
@Port INT,
@Ssl INT
AS
BEGIN
	INSERT INTO [Mtro].[Empresa]
		VALUES 
		(@RucComp,
		@RazonSoc,
		@Dir,
		@Tel,
		@Email,
		@Pass,
		@Dom,
		@IP,
		@Port,
		@Ssl)
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_RegisterUserComp]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [Seg].[Usp_RegisterUserComp]
@NroDoc VARCHAR(11),
@RazonSoc VARCHAR(250),
@Email VARCHAR(250),
@User VARCHAR(250),
@Pass VARCHAR(250)
AS
BEGIN
	INSERT INTO [Mtro].[ClienteEmpresa]
	([NroDocumento],[RazonSocial],[Email],[Username],[Password],[IdTipoUsuario],[IdEstado])
	VALUES
	(@NroDoc,
	@RazonSoc,
	@Email,
	@User,
	@Pass, 1, 1)
END

GO
/****** Object:  StoredProcedure [Seg].[Usp_RegistraEmpresaBs]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_RegistraEmpresaBs]
@RucEmpresa VARCHAR(11),
@RazonSocial VARCHAR(250),
@Direccion VARCHAR(250),
@Telefono VARCHAR(20),
@Email VARCHAR(250),
@Password VARCHAR(250)
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
/****** Object:  StoredProcedure [Seg].[Usp_UpdateEmpresaBs]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_UpdateEmpresaBs]
@IdEmpresa INT,
@RucEmpresa VARCHAR(11),
@RazonSocial VARCHAR(250),
@Direccion VARCHAR(250),
@Telefono VARCHAR(20),
@Email VARCHAR(250),
@Password VARCHAR(250)
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
/****** Object:  StoredProcedure [Seg].[Usp_ValidarExisteEmail]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [Seg].[Usp_ValidarExisteEmail] --'nyxjosue@gmail.com'
@Email VARCHAR(250)
AS
BEGIN
	SELECT
		*
	FROM [Mtro].[ClienteEmpresa]
	WHERE [Email] = @Email
END 

GO
/****** Object:  Table [Mtro].[ClienteEmpresa]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Mtro].[ClienteEmpresa](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[NroDocumento] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[RazonSocial] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Email] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Contrasenia] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Username] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Password] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[IdTipoUsuario] [int] NULL,
	[IdEstado] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Mtro].[Empresa]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Mtro].[Empresa](
	[IdEmpresa] [int] IDENTITY(1,1) NOT NULL,
	[RucEmpresa] [varchar](11) COLLATE Modern_Spanish_CI_AS NULL,
	[RazonSocial] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Direccion] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Telefono] [varchar](20) COLLATE Modern_Spanish_CI_AS NULL,
	[Email] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Password] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[DOMAIN] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[IP] [varchar](40) COLLATE Modern_Spanish_CI_AS NULL,
	[PORT] [int] NULL,
	[UseSSL] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEmpresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Mtro].[Estado]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Mtro].[Estado](
	[IdEstado] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](150) COLLATE Modern_Spanish_CI_AS NULL,
	[Codigo] [varchar](100) COLLATE Modern_Spanish_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEstado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Mtro].[TipoUsuario]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Mtro].[TipoUsuario](
	[IdTipoUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTipoUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Seg].[Usuario]    Script Date: 11/04/2017 14:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seg].[Usuario](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Nombres] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
	[Apellidos] [varchar](200) COLLATE Modern_Spanish_CI_AS NULL,
	[Dni] [varchar](50) COLLATE Modern_Spanish_CI_AS NULL,
	[Email] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Username] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[Password] [varchar](250) COLLATE Modern_Spanish_CI_AS NULL,
	[IdEstado] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [Mtro].[ClienteEmpresa] ON 
INSERT [Mtro].[ClienteEmpresa] ([IdUsuario], [NroDocumento], [RazonSocial], [Email], [Contrasenia], [Username], [Password], [IdTipoUsuario], [IdEstado]) VALUES (1, N'RUCREPLACE', N'RAZONSOCIALREPLACE', N'EMAILREPLACE', NULL, N'RUCREPLACE', N'PASSWORDREPLACE', 1, 1)
SET IDENTITY_INSERT [Mtro].[ClienteEmpresa] OFF

SET IDENTITY_INSERT [Mtro].[Empresa] ON 
INSERT [Mtro].[Empresa] ([IdEmpresa], [RucEmpresa], [RazonSocial], [Direccion], [Telefono], [Email], [Password], [DOMAIN], [IP], [PORT], [UseSSL]) VALUES (1, N'RUCREPLACE', N'RAZONSOCIALREPLACE', N'DIRECCTIONREPLACE', N'PHONEREPLACE', N'MAILCOMPANYREPLACE', N'PASSWORDCOMAPANYREPLACE', N'DOMAINREPLACE', N'IPREPLACE', PORTREPLACE, USESSLREPLACE)
SET IDENTITY_INSERT [Mtro].[Empresa] OFF
SET IDENTITY_INSERT [Mtro].[Estado] ON 

INSERT [Mtro].[Estado] ([IdEstado], [Descripcion], [Codigo]) VALUES (1, N'Activo', N'A')
INSERT [Mtro].[Estado] ([IdEstado], [Descripcion], [Codigo]) VALUES (2, N'Inactivo', N'I')
INSERT [Mtro].[Estado] ([IdEstado], [Descripcion], [Codigo]) VALUES (3, N'Bloqueado', N'B')
SET IDENTITY_INSERT [Mtro].[Estado] OFF
SET IDENTITY_INSERT [Mtro].[TipoUsuario] ON 

INSERT [Mtro].[TipoUsuario] ([IdTipoUsuario], [Descripcion], [IdEstado]) VALUES (1, N'Empresa', 1)
INSERT [Mtro].[TipoUsuario] ([IdTipoUsuario], [Descripcion], [IdEstado]) VALUES (2, N'Cliente', 1)
SET IDENTITY_INSERT [Mtro].[TipoUsuario] OFF
