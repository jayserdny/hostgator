using System.ComponentModel;

namespace MVC5_Seneca.EntityModels

{
    public class DocumentType
    {
        public int Id { get; set; }

        [DisplayName("Document Type")] 
        public string Name { get; set; } 
       
    }
}