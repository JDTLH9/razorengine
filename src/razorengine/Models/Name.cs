namespace razorengine.Models
{
    public class Name
    {
        public string MyName => "Jon";
        public Person[] Persons { get; set; }
    }

    public class Person
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
    }
}
