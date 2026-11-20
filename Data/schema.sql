-- SOLID - Esquema da base de dados SQLite
PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Aplicacao (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tipo TEXT NOT NULL CHECK (tipo = 'application'),
    name TEXT NOT NULL,
    descricao TEXT,
    data_criacao TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Canal (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tipo TEXT NOT NULL CHECK (tipo = 'channel'),
    name TEXT NOT NULL,
    descricao TEXT,
    data_criacao TEXT NOT NULL,
    aplicacao_id INTEGER NOT NULL,
    FOREIGN KEY (aplicacao_id) REFERENCES Aplicacao(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS DataRecord (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tipo TEXT NOT NULL CHECK (tipo = 'data-record'),
    name TEXT NOT NULL,
    tipo_conteudo TEXT NOT NULL,
    conteudo TEXT NOT NULL,
    data_criacao TEXT NOT NULL,
    data_expiracao TEXT,
    canal_id INTEGER NOT NULL,
    FOREIGN KEY (canal_id) REFERENCES Canal(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Subscription (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    tipo TEXT NOT NULL CHECK (tipo = 'subscription'),
    name TEXT NOT NULL,
    evt INTEGER NOT NULL CHECK (evt IN (1, 2)),
    endpoint TEXT NOT NULL,
    data_criacao TEXT NOT NULL,
    data_expiracao TEXT,
    canal_id INTEGER NOT NULL,
    FOREIGN KEY (canal_id) REFERENCES Canal(id) ON DELETE CASCADE
);

-- Os nomes dos recursos têm de ser únicos dentro do respetivo recurso-pai.
CREATE UNIQUE INDEX IF NOT EXISTS UX_Aplicacao_Name ON Aplicacao(name);
CREATE UNIQUE INDEX IF NOT EXISTS UX_Canal_Aplicacao_Name ON Canal(aplicacao_id, name);
CREATE UNIQUE INDEX IF NOT EXISTS UX_DataRecord_Canal_Name ON DataRecord(canal_id, name);
CREATE UNIQUE INDEX IF NOT EXISTS UX_Subscription_Canal_Name ON Subscription(canal_id, name);
