using CarDealership.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CarController : ControllerBase
    {
        CarsContext context = new CarsContext();

        [HttpGet("FullCarList")]

        public List<Car> GetCars()
        {
            return context.Cars.ToList();
        }
        [HttpGet("CarByIndex/{id}")]
        public Car GetCar(int id)
        {
            try
            {
                if (id > 0 && id <= context.Cars.Count())
                {
                    Car output = context.Cars.Find(id);
                    return output;

                }
                else
                {
                    Car c = new Car();
                    c.Make = $"No car with id {id} was found please input an id between 1 and {context.Cars.Count()}";
                    return c;
                }
            }
            catch (Exception ex)
            {
                Car c = new Car();
                c.Make = ex.Message;
                return c;
            }
        }
        [HttpGet("CarsByYear/{year}")]
        public List<Car> SearchByYear(int year)
        {
            List<Car> results = context.Cars.Where(x => x.Year == year).ToList();
            return results;
        }
        [HttpGet("CarsByModel/{model}")]
        public List<Car> SearchByMake(string model)
        {
            List<Car> results = context.Cars.Where(x => x.Model == model).ToList();
            return results;
        }
        [HttpPost("Create")]

        public void CreateCar(Car input)
        {
            context.Cars.Add(input);
            context.SaveChanges();
        }
        [HttpDelete("Delete/{id}")]
        public string DeleteCar(int id)
        {
            int initialCount = context.Cars.Count();
            Car toDelete = GetCar(id);

            try
            {
                context.Cars.Remove(toDelete);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                string errorOutput = ex.Message;
                errorOutput += "\n No changes made to the database";
                return errorOutput;
            }
            int finalCount = context.Cars.Count();
            return $"Initial count: {initialCount}, Final Count: {finalCount}";
        }
             [HttpPut("Update/{id}")]
        public string UpdateCar(int id, Car updatedCar)
        {
            Car Car = GetCar(id);
            //The reason I do Car.id and not just id is that GetCar if it runs properly that proves its a valid id
            Car.Id = context.Cars.Count() + 1;
            Car.Make = updatedCar.Make;
            Car.Model = updatedCar.Model;
            Car.Year = updatedCar.Year;
            Car.Color = updatedCar.Color;
            context.Cars.Update(Car);
            context.SaveChanges();
            return $"{updatedCar.Model} at id {updatedCar.Id} has been updated";
        }


    }
}
