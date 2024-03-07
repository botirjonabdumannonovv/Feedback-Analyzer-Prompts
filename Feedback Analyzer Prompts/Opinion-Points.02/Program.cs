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
            ## Advanced Instructions for Extracting Opinion Points:

                    Your task is to analyze user feedback and meticulously extract distinct opinion points, focusing exclusively on those that reflect positive or negative sentiments towards the product. These points should be directly related to user experience with the product, excluding neutral, unrelated, or actionable feedback.
                    
                    ### Detailed Extraction Requirements:
                    
                    1. Positive and Negative Focus: Identify and extract only those opinion points that express a clear positive or negative sentiment related to the product. Avoid any neutral opinions.
                    
                    2. Relevance to Product: Ensure that extracted opinions are directly related to the product. Discard any opinions that are irrelevant or tangential.
                    
                    3. Opinion Precision: Extract only the segment of the sentence that contains the opinion point itself, rather than the entire sentence, for clarity and brevity.
                    
                    4. Product Description Immunity: Do not incorporate any opinion points derived from the product description or any external references.
                    
                    5. Complex Sentence Navigation: Pay special attention to sentences with complex structures or turning points that might contain contrasting sentiments. Dissect these carefully to isolate distinct opinion points.
                    
                    6. Actionable Opinion Exclusion: Omit any feedback that suggests solutions or actions rather than expressing a direct experience or sentiment.
                    
                    7. Uncertainty Elimination: Analyze and exclude any feedback where the opinion about the product is ambiguous or uncertain.
                    
                    8. Point Separation: Separate individual opinion points clearly, even if they are part of a single sentence or connected by conjunctions.
                    
                    ### Examples for Clarification:
                    
                    These examples demonstrate how to handle feedback with complex sentiments:
                    
                    - ""Overall I think the Viper is a good mouse, but I can't afford it"" - Extract as a positive point.
                    - ""Overall I think the Viper is a good mouse, but not for me"" - Disregard, as it is neutral.
                    - ""Overall Viper is a good mouse, they said, but nope"" - Extract as a negative point.
                    
                    ### Product Context:
                    
                    - Product Description: {productDescription}
                      Use this to understand the features and scope of the product, aiding in the relevance evaluation of the feedback.
                    
                    ### Input for Opinion Mining:
                    
                    - User Feedback: {userFeedback}
                      Delve into this feedback to extract, separate, and classify the relevant positive and negative opinion points based on the outlined requirements.
                    
                    ### Expected Output:
                    
                    - Extracted Opinion Points: 
                      List out the separated and classified positive and negative opinion points from the customer feedback, adhering strictly to the guidelines provided.
                    
                    ### Strategy for Effective Extraction:
                    
                    - Employ critical analysis and discernment to differentiate between positive, negative, and neutral sentiments.
                    - Isolate the essence of each opinion point while maintaining its context and relation to the product.
                    - Ensure clarity and distinction in separating and listing the extracted opinion points.
                    
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