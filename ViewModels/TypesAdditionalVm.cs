using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WASP_F_E.ViewModels
{
    class TypesAdditionalVm
    {
        [DisplayName("Назва дослідження")]
        public string StudyName { get; set; }
        [DisplayName("Рік")]
        public int Year { get; set; }
        [DisplayName("Тип блоків")]
        public string PlantType { get; set; }
        [DisplayName("Value")]
        public double Value { get; set; }
    }
}
