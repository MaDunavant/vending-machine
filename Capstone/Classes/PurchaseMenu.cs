﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    /// <summary>
    /// represents a purchase menu of a vending machine
    /// </summary>
    public class PurchaseMenu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseMenu"/> class.
        /// constructor for a purchase menu
        /// </summary>
        /// <param name="vm">vending machine using this menu</param>
        /// <param name="mm">the main menu creating this purchase menu</param>
        public PurchaseMenu(VendingMachine vm, MainMenu mm)
        {
            this.VM = vm;
            this.MM = mm;
        }

        /// <summary>
        /// Gets Vending machine which is currently running the
        /// purchase menu
        /// </summary>
        public VendingMachine VM { get; }

        /// <summary>
        /// Gets MainMenu this purchase menu was called from
        /// </summary>
        public MainMenu MM { get; }

        /// <summary>
        /// displays the purchase menu itself
        /// </summary>
        public void Display()
        {
            // hold purchase menu on console until user acts/breaks
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Purchase Menu");
                Console.WriteLine("------------------------");
                Console.WriteLine("(1) Feed Money");
                Console.WriteLine("(2) Select Product");
                Console.WriteLine("(3) Finish Transaction");
                Console.WriteLine("(Q) Quit to Main Menu");
                Console.WriteLine();
                Console.WriteLine($"Current Money Provided: {this.VM.CurrentBalance:C2}");
                Console.WriteLine();
                Console.Write("> Pick One: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    this.GetMoney();
                }
                else if (choice == "2")
                {
                    this.GetProduct();
                }
                else if (choice == "3")
                {
                    // current vending machine calls FinishTransaction
                    int[] change = this.VM.FinishTransaction();

                    // calling dispense message method for this particular
                    // vending machine
                    this.DispenseMessage();
                    Console.WriteLine();
                    Console.WriteLine($"Your Change: {change[0]} quarters, {change[1]} dimes, and {change[2]} nickels.");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to return to Main Menu.");
                    Console.ReadKey();
                    break;
                }
                else if (choice.ToLower() == "q")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Input. Try Again. Press any key to continue.");
                    Console.ReadKey();
                }
            }
        }

        // menu for method to get money desposited by the user,
        // passing in the menu selection number, set up within
        // a while loop (ends with user selection action)

        /// <summary>
        /// initiates the menu to get money from the user
        /// </summary>
        public void GetMoney()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"Current Money Provided: {this.VM.CurrentBalance:C2}");
                Console.WriteLine();
                Console.WriteLine("(1) $1.00");
                Console.WriteLine("(2) $2.00");
                Console.WriteLine("(5) $5.00");
                Console.WriteLine("(10) $10.00");
                Console.WriteLine("(Q) Quit");
                Console.WriteLine();
                Console.Write("How much are you depositing? > $");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    this.VM.DepositMoney(1);
                }
                else if (choice == "2")
                {
                    this.VM.DepositMoney(2);
                }
                else if (choice == "5")
                {
                    this.VM.DepositMoney(5);
                }
                else if (choice == "10")
                {
                    this.VM.DepositMoney(10);
                }
                else if (choice.ToLower() == "q")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Input. Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// initiates menu to get product from user
        /// </summary>
        public void GetProduct()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter a slotID for the product you would like to purchase?");
                Console.WriteLine("(1) Display Products");
                Console.WriteLine("(Q) Quit to Purchase Menu");
                Console.WriteLine();
                Console.Write("> Slot ID: ");
                string choice = Console.ReadLine();

                // IF the user wants to return to purchase menu
                if (choice.ToLower() == "q")
                {
                    // BREAK out of GetProduct to return
                    break;
                }

                if (choice == "1")
                {
                    this.MM.DisplayProducts();
                    continue;
                }

                // IF the product does not exist in inventory
                if (!this.VM.Inventory.ContainsKey(choice))
                {
                    // THEN prompt user invalid input
                    Console.WriteLine();
                    Console.WriteLine("Invalid slotID. Press any key to return to Purchase Menu.");
                    Console.ReadKey();
                    return;
                }

                // Set VendingMachineSlot and product from user input
                VendingMachineSlot vms = this.VM.Inventory[choice];
                Product product = vms.HeldProduct;

                // IF product is SOLD OUT
                if (vms.Quantity < 1)
                {
                    // PROMPT user product SOLD OUT
                    Console.WriteLine();
                    Console.WriteLine("Item is SOLD OUT. Press any key to return to Purchase Menu.");
                    Console.ReadKey();
                    return;
                }

                // IF not enough money to purchase
                if (this.VM.CurrentBalance < product.Price)
                {
                    // PROMPT user not enough money
                    Console.WriteLine();
                    Console.WriteLine("Not enough money to purchase. Press any key to return to Purchase Menu.");
                    Console.ReadKey();
                    return;
                }

                // Purchase product successful
                this.VM.GiveProduct(choice);
                return;
            }
        }

        // method creates the message that displays a fun purchase-related message
        // after purchase is made.  Pass in a list of Product Class, named products

        /// <summary>
        /// method to show specific Yum statements specific to user's type of purchase
        /// </summary>
        public void DispenseMessage()
        {
            // loop through list, checking for produc.Type property and
            // then run through switch statement to assign proper type-specfic message.
            foreach (Product product in this.VM.ProductsPurchased)
            {
                Console.WriteLine();
                switch (product.Type.ToLower())
                {
                    case "chip":

                        Console.WriteLine("Crunch Crunch, Yum!");
                        break;

                    case "candy":

                        Console.WriteLine("Munch Munch, Yum!");
                        break;

                    case "drink":

                        Console.WriteLine("Glug Glug Yum!");
                        break;
                    case "gum":

                        Console.WriteLine("Chew Chew Yum!");
                        break;

                    default:
                        break;
                }
            }

            // empty out the List for this purchase
            this.VM.ProductsPurchased.Clear();
        }
    }
}
