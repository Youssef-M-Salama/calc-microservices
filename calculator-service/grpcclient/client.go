package grpcclient

import (
	"context"
	"log"
	"time"

	pb "calculator-service/history"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

func float64Ptr(v float64) *float64 {
	return &v
}

func SendResult(result float64) {
	conn, err := grpc.NewClient(
		"localhost:5001",
		grpc.WithTransportCredentials(insecure.NewCredentials()),
	)
	if err != nil {
		log.Println("gRPC connection failed:", err)
		return
	}
	defer conn.Close()

	client := pb.NewHistoryServiceClient(conn)

	ctx, cancel := context.WithTimeout(context.Background(), 3*time.Second)
	defer cancel()

	resp, err := client.SaveResult(ctx, &pb.SaveResultRequest{
		Result: float64Ptr(result),
	})
	if err != nil {
		log.Println("gRPC call failed:", err)
		return
	}

	log.Println("History saved:", resp.Success)
}