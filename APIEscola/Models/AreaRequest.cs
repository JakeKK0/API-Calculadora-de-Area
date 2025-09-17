namespace APIEscola.Models
{
    public class AreaRequest
    {
        // Tipo esperado: "circulo", "retangulo", "triangulo"
        public string? Tipo { get; set; }

        // Para círculo: Raio
        public double? Raio { get; set; }

    // Para retângulo: Largura e Altura
    public double? Largura { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("altura")]
    public double? AlturaRetangulo { get; set; }

    // Para triângulo: Base e Altura
    public double? Base { get; set; }
    public double? AlturaTriangulo { get; set; }
        
    // Para quadrado: LadoA e LadoB (aceita nomes com caixa diferente do JS)
    public double? LadoA { get; set; }
    public double? LadoB { get; set; }

    // Para trapézio: baseMaior, baseMenor e alturaTrapezio
    public double? BaseMaior { get; set; }
    public double? BaseMenor { get; set; }
    public double? AlturaTrapezio { get; set; }
    }
}
