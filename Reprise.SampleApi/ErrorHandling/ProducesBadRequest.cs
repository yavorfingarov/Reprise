namespace Reprise.SampleApi.ErrorHandling
{
    public class ProducesBadRequestAttribute : ProducesAttribute
    {
        public ProducesBadRequestAttribute() :
            base(StatusCodes.Status400BadRequest, typeof(ErrorResponse), null)
        {
        }
    }
}
