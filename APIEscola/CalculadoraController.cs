using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System;
using APIEscola.Models;

[ApiController]
[Route("api/[controller]")]
public class CalculadoraController : ControllerBase
{
	[HttpGet("ping")]
	public IActionResult Ping() => Ok(new { status = "ok", service = "Calculadora API" });

	[HttpPost("area")]
	public IActionResult CalcularArea([FromBody] AreaRequest req)
	{
		if (req == null || string.IsNullOrWhiteSpace(req.Tipo))
			return BadRequest(new { message = "Requisição inválida: informe o tipo." });

		var tipo = req.Tipo.Trim().ToLowerInvariant();
		switch (tipo)
		{
			case "quadrado":
				if (req.LadoA is null || req.LadoB is null || req.LadoA <= 0 || req.LadoB <= 0)
					return BadRequest(new { message = "LadoA e LadoB devem ser maiores que zero." });
				var areaQuad = req.LadoA.Value * req.LadoB.Value;
				return Ok(new { area = areaQuad });

			case "trapezio":
			case "trapézio":
				if (req.BaseMaior is null || req.BaseMenor is null || req.AlturaTrapezio is null || req.BaseMaior <= 0 || req.BaseMenor <= 0 || req.AlturaTrapezio <= 0)
					return BadRequest(new { message = "BaseMaior, BaseMenor e AlturaTrapezio devem ser maiores que zero." });
				var areaTrap = 0.5 * (req.BaseMaior.Value + req.BaseMenor.Value) * req.AlturaTrapezio.Value;
				return Ok(new { area = areaTrap });

			case "circulo":
			case "círculo":
				if (req.Raio is null || req.Raio <= 0)
					return BadRequest(new { message = "Raio deve ser maior que zero." });
				var areaCirculo = Math.PI * req.Raio.Value * req.Raio.Value;
				return Ok(new { area = areaCirculo });

			case "retangulo":
			case "retângulo":
				var larguraVal = req.Largura;
				var alturaRetLocal = req.AlturaRetangulo ?? req.AlturaTriangulo;
				if (!larguraVal.HasValue || !alturaRetLocal.HasValue)
					return BadRequest(new { message = "Largura e Altura devem ser maiores que zero." });
				var larguraDouble = larguraVal.Value;
				var alturaDouble = alturaRetLocal.Value;
				if (larguraDouble <= 0 || alturaDouble <= 0)
					return BadRequest(new { message = "Largura e Altura devem ser maiores que zero." });
				var areaRet = larguraDouble * alturaDouble;
				return Ok(new { area = areaRet });

			case "triangulo":
			case "triângulo":
				// aceitar 'altura' ou 'alturaTriangulo'
				var baseVal = req.Base;
				var alturaTri = req.AlturaRetangulo ?? req.AlturaTriangulo;
				if (!baseVal.HasValue || !alturaTri.HasValue)
					return BadRequest(new { message = "Base e Altura devem ser maiores que zero." });
				var baseDouble = baseVal.Value;
				var alturaTriDouble = alturaTri.Value;
				if (baseDouble <= 0 || alturaTriDouble <= 0)
					return BadRequest(new { message = "Base e Altura devem ser maiores que zero." });
				var areaTri = 0.5 * baseDouble * alturaTriDouble;
				return Ok(new { area = areaTri });

			default:
				return BadRequest(new { message = "Tipo desconhecido. Use: circulo, retangulo, triangulo, quadrado, trapezio." });
		}
	}
    private readonly HttpClient _httpClient;

    public CalculadoraController()
    {
        _httpClient = new HttpClient();
    }

    [HttpGet("calcular-area")]
    public async Task<IActionResult> CalcularAreaViaSite()
    {
        // 1. Consumir dados do usuário via GET
        var getUrl = "http://127.0.0.1:5500/site.html";
        // Supondo que o site retorne um JSON com os dados necessários
        var dados = await _httpClient.GetFromJsonAsync<AreaRequest>(getUrl);
        if (dados == null)
            return BadRequest("Dados não encontrados");

        // 2. Calcular área (exemplo para retângulo)
        double area = dados.Base * dados.Altura;

        // 3. Enviar resultado via POST
        var postUrl = "http://127.0.0.1:5500/api/resultado"; // Ajuste depois se necessário
        var resultado = new { area = area };
        var response = await _httpClient.PostAsJsonAsync(postUrl, resultado);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Falha ao enviar resultado");

        return Ok(resultado);
    }
}