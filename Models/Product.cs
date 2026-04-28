using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid ProductId { get; set; } = Guid.NewGuid();
    
    public string? Name { get; set; }          // Produktnavn
    public string? Description { get; set; }   // Beskrivelse
    public string? Sku { get; set; }           // SKU (lagerkode)
    public string? Brand { get; set; }         // Brand
    public string? Manufacturer { get; set; }  // Producent
    public string? Model { get; set; }         // Modelnavn
    public decimal Price { get; set; }         // Pris
    public string? Currency { get; set; }      // Valuta, fx "DKK"
    public string? ImageUrl { get; set; }      // Billede-URL
    public string? ProductUrl { get; set; }    // Link til produkt
    public DateTime ReleaseDate { get; set; }  // Udgivelsesdato
    public DateTime? ExpiryDate { get; set; }  // Udløbsdato (valgfri)
}