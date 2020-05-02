using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Server.models
{
    public class EmployeeModel
    {
        public ObjectId _id;
        public List<ObjectId> roles = new List<ObjectId>();
        public string firstName;
        public string lastName;
        public char gender;
        public Address address;
        public Contact contact;
        public DateTime birthDate;
        public string maritalStatus;
    }

    public class EmployeePopulated
    {
        public ObjectId _id;
        public List<RoleModel> roles = new List<RoleModel>();
        public string firstName;
        public string lastName;
        public char gender;
        public Address address;
        public Contact contact;
        public DateTime birthDate;
        public string maritalStatus;
    }

    public class Contact
    {
        public string homePhone;
        public string cellPhone;
        public string email;
        public string socialInsuranceNumber;
    }

    public class Address
    {
        public string streetAddress;
        public string city;
        public string province;
        public string postalCode;
    }
}
