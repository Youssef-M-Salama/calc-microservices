package main

import (
	"encoding/json"
	"log"
	"net/http"

	grpcclient "calculator-service/grpcclient"
)

type CalcRequest struct {
	A float64 `json:"a"`
	B float64 `json:"b"`
}

type CalcResponse struct {
	Result float64 `json:"result"`
}

func calculateHandler(w http.ResponseWriter, r *http.Request) {
    // reject if not from gateway
    if r.Header.Get("X-Source") != "api-gateway" {
        http.Error(w, "Forbidden", http.StatusForbidden)
        return
    }

    if r.Method != http.MethodPost {
        http.Error(w, "Method Not Allowed", http.StatusMethodNotAllowed)
        return
    }

    var req CalcRequest
    err := json.NewDecoder(r.Body).Decode(&req)
    if err != nil {
        http.Error(w, "Invalid request body", http.StatusBadRequest)
        return
    }

    result := req.A + req.B

    go grpcclient.SendResult(result)

    resp := CalcResponse{Result: result}
    w.Header().Set("Content-Type", "application/json")
    json.NewEncoder(w).Encode(resp)
}

func main() {
	http.HandleFunc("/calculate", calculateHandler)
	log.Println("Calculator Service running on port 4000")
	log.Fatal(http.ListenAndServe(":4000", nil))
}