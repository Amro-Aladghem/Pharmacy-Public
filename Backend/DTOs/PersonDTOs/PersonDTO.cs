namespace DTOs.PersonDTOs
{
    public class PersonDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName => FirstName + " " + LastName;
        public string? ProfileImageLink { get; set; }

    }
}
