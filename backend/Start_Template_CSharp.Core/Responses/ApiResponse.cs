namespace Start_Template_CSharp.Core.Responses;

public class ApiResponse<T>  
{
   public T? Data { get; private init; }
   public int TotalCount { get; private init; }
   public string? Message { get; private init; }  
   
  
   
   public static ApiResponse<T> MyResponseApi(T? data = default(T), 
                                                string? message = null,
                                                int totalCount = 0)
   {
       return new ApiResponse<T>()
       {
           Data = data,
           Message = message,
           TotalCount =  totalCount
            
           
       };

   }
}