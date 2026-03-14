    using Grpc.Core;

    public class HistoryGrpcService : History.Grpc.HistoryService.HistoryServiceBase
    //                              
    {
        private readonly HistoryDbContext _db;

        public HistoryGrpcService(HistoryDbContext db)
        {
            _db = db;
        }

        public override async Task<History.Grpc.SaveResultResponse> SaveResult(
            History.Grpc.SaveResultRequest request,
            ServerCallContext context)
        {
            if (double.IsNaN(request.Result)||!request.HasResult)
                return new History.Grpc.SaveResultResponse { Success = false };

            _db.CalculationResults.Add(new CalculationResult
            {
                Result = request.Result
            });

            await _db.SaveChangesAsync();

            return new History.Grpc.SaveResultResponse { Success = true };
        }
    }