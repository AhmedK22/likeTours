namespace LikeTours.Data.DTO.Type
{
    public class TypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lang { get; set; }
        public List<TypeDto> RefferenceTypes{ get; set; }
    }
}
