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
        var extractionPrompt = $@"""
            Advanced Instructions for Extracting Relevant User Feedback Opinions:
                    Your task is to meticulously identify and extract opinions from customer feedback that directly relate to our product. The aim is to isolate these opinions and present them in a clear and coherent manner while preserving their original context.
                    
                    Detailed Extraction Requirements:
                    Thorough Opinion Identification: Scrutinize the entirety of the feedback to locate all opinions concerning our product. Be attentive to different parts of the feedback as opinions may be dispersed throughout.
                    
                    Contextual Coherence: Transform each identified opinion into a comprehensible and standalone sentence, maintaining the original context and meaning of the feedback.
                    
                    Aggregation of Opinions: If multiple opinions are found, aggregate them into a cohesive summary. This summary should encapsulate the key points of the customer's feedback regarding the product in a logical and structured manner.
                    
                    Data Provided:
                    Product Description: {productDescription}
                    Utilize this reference to better understand which aspects of the feedback pertain to the product features, performance, or user experience.
                    Input for Processing:
                    User Feedback: {userFeedback}
                    Analyze this feedback systematically to identify and extract all opinions relevant to the product. Ensure a comprehensive examination to avoid overlooking significant details.
                    Expected Output:
                    Return only the extracted opinions from the user feedback, without adding extra information.
                    Strategic Guidelines for Extraction:
                    Pay meticulous attention to detail to capture all relevant opinions.
                    Maintain the essence and context of the original feedback while restructuring it into clear, coherent sentences.
                    Prioritize clarity and coherence in the summarization to reflect a true and comprehensive understanding of the customer's opinions regarding the product.""";

        // Invoke the Semantic Kernel for feedback extraction
        var extractionResponse = await kernel.InvokePromptAsync<string>(extractionPrompt);

        // Assuming extractionResponse is formatted correctly, create and fill the UnifiedFeedback object
        var unifiedFeedback = new FeedbackExtraction { RelevantExtraction = extractionResponse };

        // Output the extracted feedback for verification
        Console.WriteLine($"{unifiedFeedback.RelevantExtraction}");
    }
    
    public class  FeedbackExtraction
    {
        public string? RelevantExtraction { get; set; }
    }
}