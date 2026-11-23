/*
  NEXUS - Esquema inicial de SQL Server
  Ejecutar una sola vez desde SQL Server Management Studio.
*/
IF DB_ID(N'NEXUS_DB') IS NULL
    CREATE DATABASE NEXUS_DB;
GO

USE NEXUS_DB;
GO

IF OBJECT_ID('dbo.Explorador', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Explorador
    (
        IdExplorador INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Explorador PRIMARY KEY,
        Nombre NVARCHAR(60) NOT NULL CONSTRAINT UQ_Explorador_Nombre UNIQUE,
        FechaRegistro DATETIME2 NOT NULL CONSTRAINT DF_Explorador_Fecha DEFAULT SYSUTCDATETIME()
    );
END;
GO

IF OBJECT_ID('dbo.Mision', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Mision
    (
        IdMision INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Mision PRIMARY KEY,
        Codigo NVARCHAR(20) NOT NULL CONSTRAINT UQ_Mision_Codigo UNIQUE,
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(500) NOT NULL,
        RealidadObjetivo NVARCHAR(20) NOT NULL,
        Recompensa NVARCHAR(200) NOT NULL
    );
END;
GO

IF OBJECT_ID('dbo.Partida', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Partida
    (
        IdPartida INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Partida PRIMARY KEY,
        IdExplorador INT NOT NULL,
        NumeroRanura INT NOT NULL,
        FechaGuardado DATETIME2 NOT NULL CONSTRAINT DF_Partida_Fecha DEFAULT SYSUTCDATETIME(),
        Energia INT NOT NULL,
        Estabilidad INT NOT NULL,
        NivelAcceso INT NOT NULL,
        RealidadActual NVARCHAR(20) NOT NULL,
        JsonProgreso NVARCHAR(MAX) NOT NULL,
        CONSTRAINT FK_Partida_Explorador FOREIGN KEY (IdExplorador)
            REFERENCES dbo.Explorador(IdExplorador),
        CONSTRAINT UQ_Partida_Explorador_Ranura UNIQUE (IdExplorador, NumeroRanura),
        CONSTRAINT CK_Partida_Ranura CHECK (NumeroRanura BETWEEN 1 AND 3),
        CONSTRAINT CK_Partida_Energia CHECK (Energia BETWEEN 0 AND 100),
        CONSTRAINT CK_Partida_Estabilidad CHECK (Estabilidad BETWEEN 0 AND 100),
        CONSTRAINT CK_Partida_Realidad CHECK (RealidadActual IN (N'Alfa', N'Beta')),
        CONSTRAINT CK_Partida_Json CHECK (ISJSON(JsonProgreso) = 1)
    );
END;
GO

IF OBJECT_ID('dbo.ProgresoMision', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProgresoMision
    (
        IdProgreso INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ProgresoMision PRIMARY KEY,
        IdPartida INT NOT NULL,
        IdMision INT NOT NULL,
        Estado NVARCHAR(20) NOT NULL,
        DatosJson NVARCHAR(MAX) NOT NULL,
        CONSTRAINT FK_ProgresoMision_Partida FOREIGN KEY (IdPartida)
            REFERENCES dbo.Partida(IdPartida) ON DELETE CASCADE,
        CONSTRAINT FK_ProgresoMision_Mision FOREIGN KEY (IdMision)
            REFERENCES dbo.Mision(IdMision),
        CONSTRAINT UQ_ProgresoMision UNIQUE (IdPartida, IdMision),
        CONSTRAINT CK_ProgresoMision_Json CHECK (ISJSON(DatosJson) = 1)
    );
END;
GO

IF OBJECT_ID('dbo.Inventario', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Inventario
    (
        IdObjeto INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Inventario PRIMARY KEY,
        IdPartida INT NOT NULL,
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(300) NOT NULL,
        Cantidad INT NOT NULL,
        CONSTRAINT FK_Inventario_Partida FOREIGN KEY (IdPartida)
            REFERENCES dbo.Partida(IdPartida) ON DELETE CASCADE,
        CONSTRAINT CK_Inventario_Cantidad CHECK (Cantidad >= 0),
        CONSTRAINT UQ_Inventario_Partida_Objeto UNIQUE (IdPartida, Nombre)
    );
END;
GO

PRINT N'Esquema NEXUS_DB creado o verificado correctamente.';
