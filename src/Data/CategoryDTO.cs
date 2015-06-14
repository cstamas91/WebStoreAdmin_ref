using System;

namespace ITStore.Data
{
    public class CategoryDTO
    {
        public Int32 ID { get; set; }

        public String Name { get; set; }


        public override bool Equals(object obj)
        {
            return (obj is CategoryDTO) && ID == (obj as CategoryDTO).ID;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
