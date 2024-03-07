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
            ## Detailed Instructions for Language Recognition:

                    Your task is to analyze customer feedback and identify the language(s) used. This requires a keen eye for linguistic details to accurately determine the language composition within the feedback.
                    
                    ### Specific Language Identification Requirements:
                    
                    1. Sentence-Based Recognition: Only identify languages when there is a coherent and readable sentence or phrase present, not isolated words. This ensures that the recognition is based on substantial language use rather than incidental or out-of-context words.
                    
                    2. Comprehensive Listing: In cases where the feedback includes multiple languages, list each language that is represented by at least one full sentence or coherent phrase. Ensure that all languages present are accounted for, reflecting the multilingual nature of the feedback.
                    
                    ### Data Context:
                    
                    - Product Description: {productDescription}
                      While this may not directly influence language recognition, understanding the product may provide context for the feedback's language content, especially if the product targets specific linguistic demographics.
                    
                    ### Input for Analysis:
                    
                    - User Feedback: {userFeedback}
                      Review this feedback attentively to detect and list all languages used, based on the presence of coherent sentences or phrases.
                    
                    ### Expected Output:
                    
                    - Identified Languages: 
                      Enumerate the languages identified within the customer feedback. Each language listed should correspond to significant, coherent language usage within the feedback text.
                    
                    ### Guidelines for Accurate Language Recognition:
                    
                    - Carefully read through the user feedback to distinguish between different languages.
                    - Do not rely on single, isolated words for language identification; focus on sentences or significant phrases.
                    - Ensure completeness by listing all languages that form meaningful parts of the feedback.
                    
                    ## Result:
                ";

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