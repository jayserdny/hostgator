using System.ComponentModel;

namespace SenecaHeights.EntityModels

{
    public class DocumentType
    {
        public int Id { get; set; }

        [DisplayName("Document Type")] 
        public string Name { get; set; } 
       
    }
}