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
            ## Comprehensive Instructions for Personal Information Redaction:

                    Your mission is to meticulously review and redact any personal information found within the customer feedback, ensuring privacy and confidentiality while maintaining the readability of the text.
                    
                    ### Strict Redaction Requirements:
                    
                    1. Selective Redaction: Identify and redact only those words or phrases that constitute personal information (e.g., names, phone numbers, email addresses, physical addresses, social security numbers, etc.). The redaction should be precise and targeted, avoiding unnecessary removal of non-sensitive text.
                    
                    2. Redaction Format: Replace each piece of redacted personal information with an appropriate number of asterisks (***) to maintain the original word's length. This helps in preserving the sentence structure and readability.
                    
                    3. Readability Preservation: Ensure that the remaining text, post-redaction, remains coherent and understandable. The essence and context of the feedback should be intact, allowing for a clear understanding of the customer's message, minus the personal details.
                    
                    ### Data Context:
                    
                    - Product Description: {productDescription}
                      This will help you understand the context of the feedback and better differentiate between product-specific terms and personal information.
                    
                    ### Input for Redaction:
                    
                    - User Feedback: {userFeedback}
                      Scrutinize this feedback to locate and redact any personal information, following the above guidelines meticulously.
                    
                    ### Expected Outcome:
                    
                    - Redacted Feedback: 
                      Provide the sanitized version of the feedback, with all personal information effectively obscured while keeping the general message and readability of the text intact.
                    
                    ### Guiding Principles for Effective Redaction:
                    
                    - Pay close attention to detail to ensure no personal information is overlooked.
                    - Apply the redaction uniformly, using asterisks to mask sensitive details while retaining the length and structure of the original text.
                    - Review the modified feedback to confirm that it remains meaningful and informative despite the redactions.
                    
                    ## Result:
                ";

        // Invoke the Semantic Kernel for feedback extraction
        var extractionResponse = await kernel.InvokePromptAsync<string>(extractionPrompt);

        // Assuming extractionResponse is formatted correctly, create and fill the UnifiedFeedback object
        var unifiedFeedback = new FeedbackExtraction { RelevantExtraction = extractionResponse };

        // Output the extracted feedback for verification
        Console.WriteLine($"Redacted Feedback: {unifiedFeedback.RelevantExtraction}");
    }
    
    public class  FeedbackExtraction
    {
        public string? RelevantExtraction { get; set; }
    }
}