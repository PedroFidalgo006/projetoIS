-- SOLID - Dados de exemplo obrigatórios pelo enunciado
-- Executar depois de schema.sql.

PRAGMA foreign_keys = ON;

INSERT OR IGNORE INTO Aplicacao (tipo, name, descricao, data_criacao)
VALUES ('application', 'app1', 'Aplicacao principal', '2026-11-20T00:00:00');

INSERT OR IGNORE INTO Canal (tipo, name, descricao, data_criacao, aplicacao_id)
SELECT 'channel', 'channel1', 'Canal principal', '2026-11-20T00:00:00', id
FROM Aplicacao WHERE name = 'app1';

INSERT OR IGNORE INTO DataRecord
(tipo, name, tipo_conteudo, conteudo, data_criacao, data_expiracao, canal_id)
SELECT 'data-record', 'dr1', 'application/json', '{"humidity":50}',
       '2026-11-20T00:00:00', NULL, id
FROM Canal WHERE name = 'channel1';

INSERT OR IGNORE INTO Subscription
(tipo, name, evt, endpoint, data_criacao, data_expiracao, canal_id)
SELECT 'subscription', 'sub1', 1, 'http://localhost:5000/notifications',
       '2026-11-20T00:00:00', NULL, id
FROM Canal WHERE name = 'channel1';
