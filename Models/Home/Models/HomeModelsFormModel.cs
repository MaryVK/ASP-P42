using Microsoft.AspNetCore.Mvc;

// какие данные принимаем
namespace ASP_P42.Models.Home.Models
{
    public class HomeModelsFormModel
    {
        // атрибут
        // значение для UserLogin нужно взять из данных HTML-формы,
        // из поля с именем user-login
        [FromForm(Name = "user-login")]
        public String UserLogin { get; set; } = null!;


        [FromForm(Name = "user-password")]
        public String UserPassword { get; set; } = null!;


        // галочка
        [FromForm(Name = "agree")]
        public bool Agree { get; set; }


        // радиокнопка
        [FromForm(Name = "gender")]
        public String Gender { get; set; } = null!;


        // Дата
        [FromForm(Name = "birth-date")]
        public DateTime BirthDate { get; set; }


        // Цвет 
        [FromForm(Name = "color")]
        public String Color { get; set; } = null!;


        // Число 
        [FromForm(Name = "age")]
        public int Age { get; set; }


    }
}
