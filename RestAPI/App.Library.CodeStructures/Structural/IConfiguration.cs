namespace App.Library.CodeStructures.Structural {

    public interface IConfiguration<T> {

         void configure ( T target_to_configure );

    }

}