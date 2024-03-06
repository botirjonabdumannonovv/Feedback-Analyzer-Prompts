using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

// Main class
public class Program
{
    public static async Task Main(string[] args)
    {
        // Configuration setup for API key retrieval
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddUserSecrets<Program>();
        var configuration = configurationBuilder.Build();

        // Semantic Kernel setup
        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddOpenAIChatCompletion(modelId: "gpt-3.5-turbo", apiKey: configuration["OpenAiApiSettings:ApiKey"]!);
        var kernel = kernelBuilder.Build();

        // Sample product description
        const string productDescription = """
                                           Razor Viper Ultimate - wireless gaming mouse
                                                
                                                Sensitivity: 20,000DPI
                                                Tracking Speed : 650IPS
                                                Resolution accuracy : 99.6%
                                                
                                                74G LIGHTWEIGHT DESIGN
                                                Enjoy faster and smoother control with a lightweight wireless mouse designed for esports. Weighing just 74g, it achieves its weight without compromising on the build strength of its ambidextrous form factor.
                                                
                                                70 HOURS OF BATTERY LIFE
                                                Improved wireless power efficiency keeps it running at peak performance for up to 70 continuous hours—charge it just once a week to power 10 hours of daily gameplay.
                                                
                                                5 ON-BOARD MEMORY PROFILES
                                                Bring your settings anywhere and be match-ready in no time. Activate up to 5 profile configurations from its onboard memory or custom settings via cloud storage.
                                                
                                                8 PROGRAMMABLE BUTTONS
                                                Fully configurable via Razer Synapse 3, the 8 programmable buttons let you access macros and secondary functions so you can execute extended moves with ease. 
                                          """;
        // User feedback input
        Console.Write("Enter your feedback about the product: ");
        var userFeedback = Console.ReadLine();

        // Define the extraction prompt
        var extractionPrompt = $@"
            ## Advanced Instructions for Feedback Relevance Extraction:

        Your task is to intricately dissect and extract segments from customer feedback that are pertinent to our product, ensuring clarity and contextual integrity. The goal is to isolate and reformulate these segments into coherent and meaningful sentences.
        
        ### Detailed Extraction Requirements:
        
        1. Comprehensive Relevance Identification: Scrutinize the entire feedback to identify all segments that pertain directly to our product. Pay attention to different sections of the feedback, as relevant information may be dispersed throughout.
        
        2. Contextual Coherence: When extracting relevant parts, ensure that each segment is transformed into a readable and understandable sentence. Maintain the original context and meaning of the feedback, ensuring that the extracted sentences are self-sufficient and clear.
        
        3. Aggregation of Relevant Content: If multiple relevant segments are identified, concatenate these extracts into a cohesive summary. This summary should reflect the key points of the customer's feedback regarding the product, presented in a logical and structured manner.
        
        ### Data Provision:
        
        - Product Description: {productDescription}
          Use this as a reference to better understand what aspects of the feedback pertain to the product features, performance, or user experience.
        
        ### Input to Process:
        
        - User Feedback: {userFeedback}
          Analyze this feedback methodically to identify and extract all segments relevant to the product. Ensure a thorough examination to avoid overlooking significant details.
        
        ### Expected Output:
        
        - Extracted Relevance Summary: 
          Present a consolidated and readable summary composed of all relevant feedback segments. This summary should provide a clear and direct reflection of the customer's perspective on the product, based solely on the extracted relevant parts.
        
        ### Strategic Guidelines for Extraction:
        
        - Ensure meticulous attention to detail to identify all possible relevant segments.
        - Maintain the essence and context of the original feedback while restructuring into clear, coherent sentences.
        - Prioritize clarity and coherence in the summarization to reflect a true and comprehensive understanding of the customer's feedback regarding the product.
        
        ## Result:
                ";

        // Invoke the Semantic Kernel for feedback extraction
        var extractionResponse = await kernel.InvokePromptAsync<string>(extractionPrompt);

        // Assuming extractionResponse is formatted correctly, create and fill the UnifiedFeedback object
        var unifiedFeedback = new FeedbackExtraction { RelevantExtraction = extractionResponse };

        // Output the extracted feedback for verification
        Console.WriteLine($"Extracted Feedback: {unifiedFeedback.RelevantExtraction}");
    }
    
    public class  FeedbackExtraction
    {
        public string? RelevantExtraction { get; set; }
    }
}