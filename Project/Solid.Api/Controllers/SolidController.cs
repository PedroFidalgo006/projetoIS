using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace Solid.Api.Controllers;

[ApiController]
public class SolidController : ControllerBase
{
    
    private SqliteConnection AbrirLigacao()
    {
        var config = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var cs = config.GetConnectionString("DefaultConnection") ?? "Data Source=solid.db";
        var conn = new SqliteConnection(cs);
        conn.Open();
        return conn;
    }

   
    [HttpGet("/api/solid")]
    public IActionResult GetDiscovery([FromHeader(Name = "solid-discovery")] string? discovery)
    {
        if (string.IsNullOrWhiteSpace(discovery))
            return BadRequest("Header 'solid-discovery' em falta.");

        var caminhos = new List<string>();

        using var conn = AbrirLigacao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT name FROM Aplicacao ORDER BY id";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            caminhos.Add($"/api/solid/{Uri.EscapeDataString(reader.GetString(0))}");

        return Ok(caminhos);
    }

    [HttpGet("/api/solid/{app}/{ch}/subs/{sub}")]
    public IActionResult GetSubscription(
        string app, string ch, string sub,
        [FromHeader(Name = "solid-discovery")] string? discovery)
    {
        using var conn = AbrirLigacao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT s.id, s.tipo, s.name, s.evt, s.endpoint,
                   s.data_criacao, s.data_expiracao, s.canal_id
            FROM Subscription s
            JOIN Canal c      ON c.id = s.canal_id
            JOIN Aplicacao a  ON a.id = c.aplicacao_id
            WHERE (a.id = @app OR a.name = @app)
              AND (c.id = @ch  OR c.name = @ch)
              AND (s.id = @sub OR s.name = @sub)
            LIMIT 1";
        cmd.Parameters.AddWithValue("@app", app);
        cmd.Parameters.AddWithValue("@ch", ch);
        cmd.Parameters.AddWithValue("@sub", sub);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
            return NotFound($"Subscription '{sub}' não encontrada em /{app}/{ch}.");

        if (!string.IsNullOrWhiteSpace(discovery))
            return Ok(Array.Empty<string>());

        return Ok(new
        {
            id = reader.GetInt32(0),
            tipo = reader.IsDBNull(1) ? null : reader.GetString(1),
            name = reader.IsDBNull(2) ? null : reader.GetString(2),
            evt = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
            endpoint = reader.IsDBNull(4) ? null : reader.GetString(4),
            data_criacao = reader.IsDBNull(5) ? null : reader.GetString(5),
            data_expiracao = reader.IsDBNull(6) ? null : reader.GetString(6),
            canal_id = reader.GetInt32(7)
        });
    }

  
    [HttpDelete("/api/solid/{app}/{ch}/subs/{sub}")]
    public IActionResult DeleteSubscription(string app, string ch, string sub)
    {
        using var conn = AbrirLigacao();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            DELETE FROM Subscription
            WHERE id IN (
                SELECT s.id
                FROM Subscription s
                JOIN Canal c      ON c.id = s.canal_id
                JOIN Aplicacao a  ON a.id = c.aplicacao_id
                WHERE (a.id = @app OR a.name = @app)
                  AND (c.id = @ch  OR c.name = @ch)
                  AND (s.id = @sub OR s.name = @sub)
            )";
        cmd.Parameters.AddWithValue("@app", app);
        cmd.Parameters.AddWithValue("@ch", ch);
        cmd.Parameters.AddWithValue("@sub", sub);

        var apagadas = cmd.ExecuteNonQuery();
        return apagadas == 0
            ? NotFound($"Subscription '{sub}' não encontrada em /{app}/{ch}.")
            : NoContent(); // 204
    }

    
}